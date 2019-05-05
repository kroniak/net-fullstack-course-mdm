using AlfaBank.Core.Data;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedMember.Global

namespace Server.Test.Mocks
{
    public static class SqlContextMock
    {
        public static SqlContext GetSqlContext()
        {
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase("Test_database")
                .EnableSensitiveDataLogging()
                .Options;

            var context = new SqlContext(options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}