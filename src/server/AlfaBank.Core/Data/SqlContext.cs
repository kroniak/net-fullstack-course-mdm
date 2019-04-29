using System.Linq;
using AlfaBank.Core.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable UnusedMember.Global

namespace AlfaBank.Core.Data
{
    /// <inheritdoc />
    public class SqlContext : DbContext
    {
        /// <inheritdoc />
        public SqlContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Cards type Set
        /// </summary>
        public DbSet<Card> Cards { get; set; }

        /// <summary>
        /// Transactions type Set
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }

        /// <summary>
        /// Users type set
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Indexes
            modelBuilder.Entity<User>().HasIndex(u => u.UserName);
            modelBuilder.Entity<Card>().HasIndex(u => u.CardNumber);

            // Init Data
            var users = FakeDataGenerator.GenerateFakeUsers(new[]
            {
                "admin@admin.ru",
                "user@user.ru"
            }).ToArray();

            var cards = FakeDataGenerator.GenerateFakeCards(users[0]).ToArray();
            var transactions = FakeDataGenerator.GenerateFakeTransactions(cards).ToArray();

            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Card>().HasData(cards);
            modelBuilder.Entity<Transaction>().HasData(transactions);
        }
    }
}