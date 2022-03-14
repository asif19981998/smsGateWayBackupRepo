using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mysms.Models.Auth
{
    public class ApplicationIdentityUser:IdentityUser<string>
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
