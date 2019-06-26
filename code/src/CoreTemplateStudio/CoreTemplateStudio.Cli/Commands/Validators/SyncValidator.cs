// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Security.Cryptography;
using FluentValidation;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class SyncValidator : AbstractValidator<SyncCommand>
    {
        public SyncValidator()
        {
            RuleFor(c => c.Path)
                .NotEmpty()
                .WithMessage(StringRes.BadReqInvalidPath);

            RuleFor(c => c.FullPath)
                .NotEmpty()
                .Must(f => IsValidPath(f))
                .WithMessage(StringRes.BadReqInvalidPath);

            RuleFor(c => c.FullPath)
                    .Must(f => !f.EndsWith("mstx") || IsPackageHashValid(f))
                    .WithMessage(StringRes.BadReqInvalidPackage);

            RuleFor(c => c.Platform)
                .NotEmpty()
                .Must(p => Platforms.IsValidPlatform(p))
                .WithMessage(StringRes.BadReqInvalidPlatform);

            RuleFor(x => x)
                .Must(x => ProgrammingLanguages.IsValidLanguage(x.Language, x.Platform))
                .WithMessage(StringRes.BadReqInvalidLanguage);
        }

        private bool IsPackageHashValid(string fullpath)
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

        private bool IsValidPath(string fullpath)
        {
#if DEBUG
            return fullpath != null && Directory.Exists(fullpath);
#else
            return fullpath != null && File.Exists(fullpath);
#endif
        }
    }
}
