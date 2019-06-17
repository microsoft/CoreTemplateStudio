using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Cli.Options;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Cli.Utilities;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Cli.Services
{
    public class SyncService : ISyncService
    {
        private readonly IMessageService _messageService;

        private readonly string _platform;
        private readonly string _language;
        private string _path;
        private string _fullPath;

        private bool _wasUpdated;

        public SyncService(IMessageService messageService)
        {
            _messageService = messageService;

            _platform = "Web";
            _language = "Any";
        }

        public async Task<int> ProcessAsync(SyncOptions options)
        {
            ConfigurePaths(options.Path);

            if (!Platforms.IsValidPlatform(_platform))
            {
                _messageService.SendError(StringRes.BadReqInvalidPlatform);
                return 0;
            }

            if (!IsValidPath(_fullPath))
            {
                _messageService.SendError(StringRes.BadReqInvalidPath);
                return 0;
            }

            if (_fullPath.EndsWith("mstx") && !IsPackageValid(_fullPath))
            {
                _messageService.SendError(StringRes.BadReqInvalidPackage);
                return 0;
            }

            if (!ProgrammingLanguages.IsValidLanguage(_language, _platform))
            {
                _messageService.SendError(StringRes.BadReqInvalidLanguage);
                return 0;
            }

            try
            {
#if DEBUG
                GenContext.Bootstrap(
                    new LocalTemplatesSource(
                        _path,
                        "0.0.0.0",
                        string.Empty),
                    new CliGenShell(),
                    new Version("0.0.0.0"),
                    _platform,
                    _language);
#else
                GenContext.Bootstrap(
                  new RemoteTemplatesSource(
                      _platform,
                      _language,
                      _path,
                      new CliDigitalSignatureService()),
                  new ApiGenShell(),
                  new Version(GetFileVersion()),
                  _platform,
                  _language);
#endif
                GenContext.ToolBox.Repo.Sync.SyncStatusChanged += OnSyncStatusChanged;
                await GenContext.ToolBox.Repo.SynchronizeAsync(true);

                var syncModel = new { GenContext.ToolBox.TemplatesVersion, WasUpdated = _wasUpdated };
                _messageService.Send(syncModel);
                //return new SyncModel()
                //{
                //    TemplatesVersion = GenContext.ToolBox.TemplatesVersion,
                //    WasUpdated = _wasUpdated,
                //};
            }
            catch (Exception ex)
            {
                _messageService.SendError(string.Format(StringRes.ErrorSyncingTemplates, ex.Message));
            }

            return 0;
        }

        private static string GetFileVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            return fvi.FileVersion;
        }

        private bool IsPackageValid(string fullpath)
        {
            return Configuration.Current.AllowedPackages.Contains(GetHash(fullpath));
        }

        private string GetHash(string fullpath)
        {
            using (FileStream stream = File.OpenRead(fullpath))
            {
                var sha = new SHA256Managed();
                byte[] checksum = sha.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", string.Empty);
            }
        }

        private void OnSyncStatusChanged(object sender, SyncStatusEventArgs args)
        {
            //SignalR call
            //Clients.Caller.SendAsync("syncMessage", status.ToString(), progress);
            _messageService.SendMessage($"syncMessage : {args.Status.ToString()} - {args.Progress}");

            if (args.Status.Equals(SyncStatus.Updated))
            {
                _wasUpdated = true;
            }
        }

        private bool IsValidPath(string fullpath)
        {
#if DEBUG
            return fullpath != null && Directory.Exists(fullpath);
#else
            return fullpath != null && File.Exists(fullpath);
#endif
        }

        private void ConfigurePaths(string path)
        {
#if DEBUG
            _path = path;
            _fullPath = path + "/templates";

#else
            _path = @"..";
            _fullPath = Path.Combine(_path, $"{_platform}.{_language}.Templates.mstx");
#endif
        }
    }
}