using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using AlfaBank.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlfaBank.Core.Data.Repositories
{
    /// <inheritdoc />
    /// <summary>
    /// Base realisation Repository for EF
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    [ExcludeFromCodeCoverage]    
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly SqlContext _context;
        private readonly DbSet<TEntity> _collection;
        private readonly IQueryable<TEntity> _queryableAsNoTracking;

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        protected Repository(SqlContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _collection = context.Set<TEntity>();
            _queryableAsNoTracking = _collection.AsNoTracking();
        }

        /// <inheritdoc />
        public void Add(TEntity entity) => _collection.Add(entity);

        /// <inheritdoc />
        public void Delete(int id)
        {
            var entityToDelete = _collection.Find(id);

            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _collection.Attach(entityToDelete);
            }

            _collection.Remove(entityToDelete);
        }

        /// <inheritdoc />
        public void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State != EntityState.Detached)
            {
                _collection.Remove(entity);
            }
        }

        /// <inheritdoc />
        public TEntity Get(int id) => _collection.Find(id);

        /// <inheritdoc />
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true)
            => (noTracking
                    ? _queryableAsNoTracking
                    : _collection)
                .Where(predicate);

        /// <inheritdoc />
        public TEntity GetOne(Expression<Func<TEntity, bool>> predicate, bool noTracking = true)
            => Get(predicate, noTracking)
                .FirstOrDefault();

        /// <inheritdoc />
        public IQueryable<TEntity> GetWithInclude(
            Expression<Func<TEntity, bool>> predicate,
            bool noTracking = true,
            params Expression<Func<TEntity, object>>[] includeObjects)
        {
            var query = noTracking ? _queryableAsNoTracking : _collection;
            return includeObjects
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty))
                .Where(predicate);
        }

        /// <inheritdoc />
        public IQueryable GetAll() => _queryableAsNoTracking;

        /// <inheritdoc />
        public void Update(TEntity entity)
        {
            _collection.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        /// <inheritdoc />
        public void Save() => _context.SaveChanges();
    }
}