using System;
using System.Linq;
using System.Linq.Expressions;

namespace AlfaBank.Core.Data.Interfaces
{
    /// <summary>
    /// Generic repository for work with EF
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get Entities by expressions
        /// </summary>
        /// <param name="noTracking">flag is noTracking</param>
        /// <returns>Enumerable of Entities</returns>
        IQueryable<TEntity> Get(bool noTracking = true);

        /// <summary>
        /// Get Entities by expressions
        /// </summary>
        /// <param name="predicate">filter</param>
        /// <param name="noTracking">flag is noTracking</param>
        /// <returns>Enumerable of Entities</returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true);

        /// <summary>
        /// Get Entities with include nested elements
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeObjects"></param>
        /// <param name="noTracking">flag is query is NoTracking</param>
        /// <returns>Enumerable of Entities</returns>
        IQueryable<TEntity> GetWithInclude(
            Expression<Func<TEntity, bool>> predicate,
            bool noTracking = true,
            params Expression<Func<TEntity, object>>[] includeObjects);

        /// <summary>
        /// Add Entity to DbSet
        /// </summary>
        /// <param name="entity">A Entity</param>
        void Add(TEntity entity);

        /// <summary>
        /// Update Entity in DbSet
        /// </summary>
        /// <param name="entity">A Entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Save all changes
        /// </summary>
        void Save();
    }
}