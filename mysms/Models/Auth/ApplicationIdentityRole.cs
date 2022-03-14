using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mysms.Models.Auth
{
    public class ApplicationIdentityRole:IdentityRole<string>
    {
        public string Description { get; set; }
    }
}
