using System.Collections.Generic;
using System.Linq;
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

        public static bool Tracked<TEntity>(this DbContext ctx, TEntity entity)
            where TEntity : class =>
            ctx.Set<TEntity>().Local.Contains(entity);

        public static bool Tracked<TEntity>(this DbContext ctx, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            var local = ctx.Set<TEntity>().Local;
            return entities.All(e => local.Contains(e));
        }
    }
}