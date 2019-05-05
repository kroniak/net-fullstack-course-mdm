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
        /// Get one Entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity object</returns>
        TEntity Get(int id);

        /// <summary>
        /// Get Entities by expressions
        /// </summary>
        /// <param name="predicate">filter</param>
        /// <param name="noTracking">flag is noTracking</param>
        /// <returns>Enumerable of Entities</returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true);

        /// <summary>
        /// Get Entity by expressions
        /// </summary>
        /// <param name="predicate">filter</param>
        /// <param name="noTracking">flag is noTracking</param>
        /// <returns>Entity</returns>
        TEntity GetOne(Expression<Func<TEntity, bool>> predicate, bool noTracking = true);

        /// <summary>
        /// Get all Entities
        /// </summary>
        /// <returns>Enumerable of Entities</returns>
        IQueryable GetAll();

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
        /// Delete Entity from DbSet
        /// </summary>
        /// <param name="id">Id of the entity</param>
        void Delete(int id);

        /// <summary>
        /// Delete Entity from DbSet
        /// </summary>
        /// <param name="entity">Entity self</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Save all changes
        /// </summary>
        void Save();
    }
}