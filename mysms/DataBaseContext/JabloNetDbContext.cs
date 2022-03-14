using Microsoft.EntityFrameworkCore;
using mysms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mysms.DataBaseContext
{
    public class JabloNetDbContext : DbContext
    {
        public JabloNetDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ObjectData> @object {get;set;}

        string tableName;
     
    }
}
