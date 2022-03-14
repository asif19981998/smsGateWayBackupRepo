using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mysms.Models
{
    public class Objects
    {
        public int Id { get; set; }
        public string Synonym { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        
        public string PhoneNo { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Watch { get; set; }
        public string Close { get; set; }
        public string Status { get; set; }
       
        public string Sender { get; set; }



        public int? GroupId { get; set; }
        public Group Group { get; set; }
   
    }
}
