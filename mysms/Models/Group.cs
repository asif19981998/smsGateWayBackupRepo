using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace mysms.Models
{
    public class Group
    {
        public Group()
        {
            Contacts = new List<Objects>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Objects> Contacts { get; set; }
    }
}
