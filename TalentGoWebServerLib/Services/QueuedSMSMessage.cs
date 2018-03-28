using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Services
{
    public class QueuedSMSMessage
    {
        public int Id { get; set; }

        public string[] To { get; set; }

        public string Content { get; set; }

        public string[] TemplateId { get; set; }

        public object[] Args { get; set; }

        public DateTime WhenQueued { get; set; }

        public DateTime? WhenSent { get; set; }

        public string State { get; set; }
    }
}
