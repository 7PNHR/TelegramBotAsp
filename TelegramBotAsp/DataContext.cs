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
            Database.Migrate();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=root;database=bot_db;", 
                new MySqlServerVersion(new Version(8, 0, 26))
            );
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<TextTemplate> Templates { get; set; }

    }
}
