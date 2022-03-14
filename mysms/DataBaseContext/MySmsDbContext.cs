using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mysms.Models;
using mysms.Models.Auth;
using mysms.Models.ViewModel;

namespace mysms.DataBasebContext
{
    public class MySmsDbContext:DbContext
    {
        
        public MySmsDbContext(DbContextOptions<MySmsDbContext> options):base(options)
        {

        }
     
        public DbSet<SmsInbox> smsInbox { get; set; }
        public DbSet<Group> groups { get; set; }
        public DbSet<Objects> objects { get; set; }
        public DbSet<SendItem> sendItems { get; set; }
        public DbSet<SysLog> sysLogs { get; set; }
        public DbSet<SmsInbox_Raw> smsInboxRaw { get; set; }
        public DbSet<DailyReportOpening> dailyReportOpening { get; set; }
        public DbSet<DailyReportClosing> dailyReportClosing { get; set; }

        public DbSet<PortSetting> PortSettings { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //   optionsBuilder.UseSqlServer(@"Server=DESKTOP-439FE02\SQLEXPRESS;Database=SMSSystemDb;Trusted_Connection=True;");
        //   //optionsBuilder.UseSqlServer(@"Server=192.168.88.35,1433;Database=JabloNET;User Id=sa;password=SQLjablonet2020;Trusted_Connection=False;MultipleActiveResultSets=true;");
        //   // optionsBuilder.UseSqlServer(@"Server=192.168.91.200,1433;User Id=sa;password=Admin@12345;Database=SMSSystemDb;Trusted_Connection=False;MultipleActiveResultSets=true;");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SmsInbox>().Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<SmsInbox_Raw>().Metadata.SetIsTableExcludedFromMigrations(true);
            SeedData.SeedData.SeedDatas(modelBuilder);
           
            
            modelBuilder.Entity<Objects>()
                .HasIndex(u => u.PhoneNo)
                .IsUnique();
        }
    }
}
