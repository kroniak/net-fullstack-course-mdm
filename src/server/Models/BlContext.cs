using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class BlContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        public BlContext(DbContextOptions<BlContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // наш любимый Fluent API
            // установим уникальность
            modelBuilder.Entity<User>().HasAlternateKey(u => u.UserName);
            modelBuilder.Entity<User>().HasAlternateKey(u => u.UserPasport);
            modelBuilder.Entity<Card>().HasAlternateKey(u => u.CardNumber);
            modelBuilder.Entity<Card>().HasAlternateKey(u => new { u.CardNumber, u.CardName });
            //Установим некластерные индексы
            //modelBuilder.Entity<Card>().HasIndex(u => new { u.CardNumber, u.CardName});
            modelBuilder.Entity<Transaction>().HasIndex(u => u.CardNumber_From);
            modelBuilder.Entity<Transaction>().HasIndex(u => u.CardNumber_To);
        }
    }
}
