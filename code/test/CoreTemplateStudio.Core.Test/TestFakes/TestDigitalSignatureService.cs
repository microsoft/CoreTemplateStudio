using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Templates.Core.Packaging;

namespace Microsoft.Templates.Core.Test.TestFakes
{
    class TestDigitalSignatureService : IDigitalSignatureService
    {
        public Dictionary<string, X509Certificate> GetPackageCertificates(Package package)
        {
            var packageCertificates = new Dictionary<string, X509Certificate>();
            packageCertificates.Add("Test", new X509Certificate(@"Packaging\TestCert.pfx", "pass@word1"));
            return packageCertificates;
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
            /*
            if (cert == null)
            {
                throw new ArgumentNullException("certificate");
            }

            X509ChainStatusFlags status = X509ChainStatusFlags.NoError;
            
            X509Chain chain = new X509Chain();

            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            bool isValid = chain.Build(new X509Certificate2(cert.Handle));
            if (!isValid)
            {
                foreach (var chainStat in chain.ChainStatus)
                {
                    status |= chainStat.Status;
                }
            }

            return status;
            */
            
        }

        public bool VerifySignatures(Package package)
        {
            return true;
        }
    }
}
