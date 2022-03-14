using Microsoft.EntityFrameworkCore;
using mysms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mysms.SeedData
{
    public static class SeedData
    {
        public static void SeedDatas(ModelBuilder builder)
        {
            builder.Entity<PortSetting>().HasData(
                new PortSetting { Id = 1, Port_1 = "Bank 1", Port_2 = "Bank 2", Port_3 = "Bank 3", Port_4 = "Jwl 1" }

                );

        }
    }
}
