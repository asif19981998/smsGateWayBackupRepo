using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mysms.Models
{
    public class SysLog
    {
        public int Id { get; set; }
        public string LogType { get; set; }
        public DateTime Time { get; set; }
        public string IpAddress { get; set; }
        public  string User { get; set; }
    }
}
