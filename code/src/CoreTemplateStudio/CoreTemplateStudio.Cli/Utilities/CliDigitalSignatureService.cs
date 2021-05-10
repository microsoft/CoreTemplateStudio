// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Templates.Core.Packaging;

// Modify or replace this class when dotnet core 3.0 is released to properly test
namespace Microsoft.Templates.Cli.Utilities
{
    public class CliDigitalSignatureService : IDigitalSignatureService
    {
        public bool CanVerifySignatures => false;

        public Dictionary<string, X509Certificate> GetPackageCertificates(Package package)
        {
            return new Dictionary<string, X509Certificate>();
        }

        public IEnumerable<X509Certificate> GetX509Certificates(Package package)
        {
            return new List<X509Certificate>();
        }

        public bool IsSigned(Package package)
        {
            return true;
        }

        public void SignPackage(Package package, X509Certificate cert)
        {
        }

        private void SignUris(IEnumerable<Uri> uris, X509Certificate2 cert)
        {
        }

        public X509ChainStatusFlags VerifyCertificate(X509Certificate cert)
        {
            return X509ChainStatusFlags.NoError;
        }

        public bool VerifySignatures(Package package)
        {
            return true;
        }
    }
}
