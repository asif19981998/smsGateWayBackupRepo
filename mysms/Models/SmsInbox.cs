using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mysms.Models
{
    public class SmsInbox
    {
        public int ID { get; set; }
        public string GsmSpan { get; set; }
        public string Sender { get; set; }
        public DateTime Recvtime { get; set; }
        public string SIndex { get; set; }
        public string Total { get; set; }
        public string Smsc { get; set; }
        public string Content { get; set; }

    }
}
