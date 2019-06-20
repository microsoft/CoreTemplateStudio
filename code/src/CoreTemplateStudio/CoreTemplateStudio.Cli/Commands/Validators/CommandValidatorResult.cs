using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Templates.Cli.Commands.Validators
{
    public class CommandValidatorResult
    {
        public bool IsValid => !Messages.Any();

        public List<string> Messages { get; set; } = new List<string>();

        public void AddMessage(string message) => Messages.Add(message);
    }
}
