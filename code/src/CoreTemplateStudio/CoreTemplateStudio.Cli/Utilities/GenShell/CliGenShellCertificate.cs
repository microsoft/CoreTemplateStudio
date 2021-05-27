// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.Cli.Utilities.GenShell
{
    public class CliGenShellCertificate : IGenShellCertificate
    {
        public string CreateCertificate(string publisherName) => CliCertificateService.Instance.CreateCertificate(publisherName);
    }
}
