using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mysms.Models.ViewModel
{
    public class GroupWiseSmsSend
    {
        public int Id { get; set; }
        public string Port { get; set; }
        public string PhoneNo { get; set; }
        public string Content { get; set; }
    }
}
