using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mysms.Models
{
    public class DailyReportClosing
    {
        public int Id { get; set; }
        public string SystemId { get; set; }
        public string PhoneNo { get; set;}
        public string ObjectName { get; set; }
        public string ARC { get; set; }
        public DateTime? ArcStopTime { get; set; }
        public string SMS { get; set; }
        public DateTime? SmsStopTime { get; set; }
        public string Remarks { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
