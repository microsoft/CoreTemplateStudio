using System.Collections.Generic;

namespace WtsTelemetry.Models
{
    public class SendGridConfig
    {
        public string From { get; set; }
        public IEnumerable<string> To { get; set; }
        public string Subject { get; set; }
    }
}
