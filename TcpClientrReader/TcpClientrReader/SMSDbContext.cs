using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpClientrReader
{
    public class SMSDbContext:DbContext
    {
        //public SMSDbContext(DbContextOptions<SMSDbContext> options) : base(options)
        //{

        //}

        public DbSet<SmsInbox> smsInbox { get; set; }
        public DbSet<SmsInbox_Raw> smsInboxRaw { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-439FE02\SQLEXPRESS;Database=SMSSystemDb;Trusted_Connection=True;");
            //optionsBuilder.UseSqlServer(@"Server=192.168.91.200,1433;User Id=sa;password=Admin@12345;Database=SMSSystemDb;Trusted_Connection=False;MultipleActiveResultSets=true;");
        }
    }

    
}
