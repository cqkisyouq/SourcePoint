using SourcePoint.Data.BaseData.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SourcePoint.Data.BaseData.Bases
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public abstract IQueryable<TEntity> Queryable { get; }
        public abstract IQueryable<TEntity> QueryableNoTracking { get; }

        public abstract int Delete(TEntity entity);
        public abstract int Delete(IEnumerable<TEntity> entities);
        public abstract IList<TEntity> GetEntities(Expression<Func<TEntity, bool>> expression);
        public abstract IList<TEntity> GetEntities(IEnumerable<object> ids);
        public abstract TEntity GetEntity(object id);
        public abstract TEntity Insert(TEntity entity);
        public abstract IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities);
        public abstract int Remove(TEntity entity);
        public abstract int Remove(IEnumerable<TEntity> entities);
        public abstract int SaveChanges();
        public abstract TEntity Update(TEntity entity);
        public abstract IEnumerable<TEntity> Update(IEnumerable<TEntity> entities);
    }
}
