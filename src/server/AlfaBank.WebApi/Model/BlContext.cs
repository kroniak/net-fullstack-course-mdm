using AlfaBank.WebApi.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlfaBank.Core.Models
{
    class BlContext : DbContext
    {
        public DbSet<CardDb> CardsDb { get; set; }
        public DbSet<TransactionDb> TransactionsDb { get; set; }
        public DbSet<UserDb> UsersDb { get; set; }

        public BlContext(DbContextOptions<BlContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // наш любимый Fluent API
            // установим уникальность
            modelBuilder.Entity<UserDb>().HasAlternateKey(u => u.UserName);
            modelBuilder.Entity<CardDb>().HasAlternateKey(u => u.CardNumber);
            modelBuilder.Entity<CardDb>().HasAlternateKey(u => new { u.CardNumber, u.CardName });
            //Установим некластерные индексы
            //modelBuilder.Entity<Card>().HasIndex(u => new { u.CardNumber, u.CardName});
            modelBuilder.Entity<TransactionDb>().HasIndex(u => u.DateTime);
            //modelBuilder.Entity<Transaction>().HasIndex(u => u.CardNumber_To);

            //тестовые данные
            modelBuilder.Entity<UserDb>().HasData(new UserDb { Id=1,UserName = "iocsha", Surname = "surname", Firstname = "Firstname" });
            modelBuilder.Entity<CardDb>().HasData(new CardDb { Id = 1, CardNumber = "6271190189011743", CardName = "CardName", Balance = 0, UserDbId =1});
            modelBuilder.Entity<TransactionDb>().HasData(new TransactionDb { Id = 1, CardDbId = 1, Sum = 10, CardFromNumber = "6271190189011743", CardToNumber= "6271190189011743" });
        }
    }
}
