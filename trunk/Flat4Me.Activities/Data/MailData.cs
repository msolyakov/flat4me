using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Activities.Data
{
    public class MailData
    {
        public string From { get; set; }
        public IEnumerable<string> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }

        public MailData()
        {
            IsBodyHtml = false;
        }
    }
}
