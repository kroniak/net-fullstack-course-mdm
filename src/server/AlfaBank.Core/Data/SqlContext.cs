using AlfaBank.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AlfaBank.Core.Data
{
    /// <inheritdoc />
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}