using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mysms.Models.ViewModel
{
    public class ReceivedSmsSearch
    {
        public string Synonym { get; set; }
        public string Sender { get; set; }
        public string Content { get; set; }
        public int Port { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int StatusCode { get; set; }

    }
}
