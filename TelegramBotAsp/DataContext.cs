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
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<TextTemplate> Templates { get; set; }
        public DbSet<Log> Logs { get; set; }

    }
}
