using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mysms.Models
{
    public class SendItem
    {
        public int Id { get; set; }
        public int Port { get; set; }
        public string PhoneNo { get; set; }
        public string Message { get; set; }
        public string MessageId { get; set; }
        public DateTime SendingTime { get; set; }
        public DateTime DeliveredTime { get; set; }
        public int StatusId { get; set; }
    }
}
