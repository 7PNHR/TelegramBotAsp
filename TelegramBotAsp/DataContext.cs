using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
                
        }

        public DbSet<AppUser> Users { get; set; }

    }
}
