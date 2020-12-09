// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Templates.Core.Locations;
using WtsPackagingTool.CommandOptions;

namespace WtsPackagingTool
{
    public static class RemoteSource
    {
        public static TemplatesSourceConfig GetTemplatesSourceConfig(string storageAccount, EnvEnum environment)
        {
            string env = environment.ToString().ToLowerInvariant();

            BlobContainerClient container = GetContainerAnonymous(storageAccount, env);
            var remoteElements = RemoteSource.GetAllElements(container);
            var remotePackages = remoteElements.Where(e => e != null && e.Name.StartsWith(env, StringComparison.OrdinalIgnoreCase) && e.Name.EndsWith(".mstx", StringComparison.OrdinalIgnoreCase))
                .Select((e) =>
                     {
                         return new TemplatesPackageInfo()
                         {
                             Name = e.Name,
                             Bytes = e.Properties.ContentLength.Value,
                             Date = e.Properties.LastModified.Value.DateTime,
                             Platform = e.Metadata.ContainsKey("platform") ? e.Metadata["platform"] : string.Empty,
                             Language = e.Metadata.ContainsKey("language") ? e.Metadata["language"] : string.Empty,
                             WizardVersions = e.Metadata.ContainsKey("wizardversion") ? e.Metadata["wizardversion"].Split(';').Select(v => new Version(v)).ToList() : new List<Version>(),
                         };
                     })
                .OrderByDescending(e => e.Date)
                .OrderByDescending(e => e.Version)
                .GroupBy(e => new { e.MainVersion, e.Language, e.Platform })
                .Select(e => e.FirstOrDefault());

            TemplatesSourceConfig config = new TemplatesSourceConfig()
            {
                Latest = remotePackages.FirstOrDefault(),
                Versions = remotePackages.ToList(),
                RootUri = container.Uri,
            };
            return config;
        }

        public static string UploadTemplatesContent(string storageAccount, string key, string env, string sourceFile, string version, string platform, string language, string wizardVersion)
        {
            if (!File.Exists(sourceFile))
            {
                throw new ArgumentException($"Invalid parameter '{nameof(sourceFile)}' value. The file '{sourceFile}' does not exists.");
            }

            if (string.IsNullOrWhiteSpace(sourceFile))
            {
                throw new ArgumentException($"Parameter '{nameof(sourceFile)}' can not be null, empty or whitespace.");
            }

            string blobName = GetBlobName(env, sourceFile, version);

            var container = GetContainer(storageAccount, key, env);
            var metaData = new Dictionary<string, string>()
            {
                { "platform", platform },
                { "language", language },
                { "wizardversion", wizardVersion },
            };
            return UploadElement(container, sourceFile, blobName, metaData);
        }

        public static string UploadElement(string storageAccount, string key, string env, string sourceFile)
        {
            if (!File.Exists(sourceFile))
            {
                throw new ArgumentException($"Invalid parameter '{nameof(sourceFile)}' value. The file '{sourceFile}' does not exists.");
            }

            if (string.IsNullOrWhiteSpace(sourceFile))
            {
                throw new ArgumentException($"Parameter '{nameof(sourceFile)}' can not be null, empty or whitespace.");
            }

            string blobName = Path.GetFileName(sourceFile);
            var container = GetContainer(storageAccount, key, env);
            return UploadElement(container, sourceFile, blobName, null);
        }

        public static string DownloadCdnElement(string cndUrl, string elementName, string destination)
        {
            Uri elementUri = new Uri($"{cndUrl}/{elementName}");
            string destFile = Path.Combine(destination, elementName);

            var wc = new WebClient();
            wc.DownloadFile(elementUri, destFile);

            return destFile;
        }

        private static EnvEnum ParseEnv(string name)
        {
            EnvEnum parsedEnv = EnvEnum.Unknown;
            string pattern = @"^pro|pre|dev|test";
            Regex regex = new Regex(pattern, RegexOptions.Compiled & RegexOptions.IgnoreCase & RegexOptions.CultureInvariant);

            var match = regex.Match(name);
            if (match.Success)
            {
                Enum.TryParse(match.Value, true, out parsedEnv);
            }

            return parsedEnv;
        }

        private static BlobContainerClient GetContainer(string account, string key, string container)
        {
            StorageSharedKeyCredential credentials = new StorageSharedKeyCredential(account, key);

            BlobServiceClient blobClient = new BlobServiceClient(new Uri($"https://{account}.blob.core.windows.net"), credentials);

            BlobContainerClient blobContainer = blobClient.GetBlobContainerClient(container);

            return blobContainer;
        }

        private static BlobContainerClient GetContainerAnonymous(string account, string container)
        {
            BlobServiceClient blobClient = new BlobServiceClient(new Uri($"https://{account}.blob.core.windows.net"));

            BlobContainerClient blobContainer = blobClient.GetBlobContainerClient(container);

            return blobContainer;
        }

        private static IEnumerable<BlobItem> GetAllElements(BlobContainerClient container)
        {
            if (!container.Exists())
            {
                throw new ArgumentException($"The container {container.Uri} does not exists or is not public.");
            }

            var resultSegment = container.GetBlobs(BlobTraits.Metadata, BlobStates.None).AsPages(default, 1000);

            foreach (Azure.Page<BlobItem> blobPage in resultSegment)
            {
                foreach (BlobItem blobItem in blobPage.Values)
                {
                    yield return blobItem;
                }
            }
        }

        private static string GetBlobName(string env, string sourceFile, string version)
        {
            Version specifedVersion;
            if (!Version.TryParse(version, out specifedVersion))
            {
                throw new ArgumentException($"The value '{version}' is not valid for parameter '{nameof(version)}'.");
            }

            Version versionInFile = TemplatesPackageInfo.ParseVersion(Path.GetFileName(sourceFile));
            if (versionInFile != null && versionInFile != specifedVersion)
            {
                throw new ArgumentException($"Parameter '{nameof(sourceFile)}' (with value '{sourceFile}') contains the version {versionInFile.ToString()} that do not match with the value specified in the parameter '{nameof(version)}' (with value '{version}').");
            }

            var envInFile = ParseEnv(Path.GetFileNameWithoutExtension(sourceFile));
            string prefix = string.Empty;
            if (!envInFile.ToString().Equals(env, StringComparison.OrdinalIgnoreCase) || envInFile == EnvEnum.Unknown)
            {
                prefix = env + ".";
            }

            string blobName = (versionInFile == null) ? $"{prefix}{Path.GetFileNameWithoutExtension(sourceFile)}_{version}.mstx" : sourceFile;
            return blobName;
        }

        private static string UploadElement(BlobContainerClient container, string sourceFile, string blobName, Dictionary<string, string> metaData)
        {
            BlockBlobClient blob = container.GetBlockBlobClient(blobName);

            var totalFileSize = new FileInfo(sourceFile).Length;
            var stopwatch = Stopwatch.StartNew();

            using (var fileStream = File.Open(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BlobUploadOptions uploadOptions = new BlobUploadOptions()
                {
                    TransferOptions = new StorageTransferOptions()
                    {
                        MaximumConcurrency = 4,
                    },
                };

                blob.Upload(fileStream, uploadOptions);
                if (metaData != null)
                {
                    blob.SetMetadata(metaData);
                }
            }

            stopwatch.Stop();
            return $"Uploaded {Math.Round(totalFileSize / 1024f, 2)} Kbytes in {stopwatch.Elapsed.TotalSeconds} seconds.";
        }
    }
}
