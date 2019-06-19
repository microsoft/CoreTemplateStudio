using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class CommandValidatorResult
    {
        public bool IsValid { get; set; } = true;

        public IEnumerable<string> Messages { get; set; }
    }
}
