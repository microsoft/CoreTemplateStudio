using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Templates.Cli.Models
{
    public class SyncModel
    {
        public bool WasUpdated { get; set; }

        public string TemplatesVersion { get; set; }
    }
}
