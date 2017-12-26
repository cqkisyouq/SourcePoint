using SourcePoint.Data.BaseData.Interface;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SourcePoint.Data.BaseData.Bases
{
    public  class BaseService<TEntity> : IBaseService<TEntity> where TEntity:class
    {
        protected IRepository<TEntity> _repository;
        public BaseService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual bool Delete(TEntity entity)
        {
             _repository.Delete(entity);
            return _repository.SaveChanges() == 1;
        }

        public virtual bool Delete(IEnumerable<TEntity> entities)
        {
            _repository.Delete(entities);
            return _repository.SaveChanges()>0;
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return _repository.QueryableNoTracking.Where(expression).Any();
        }

        public virtual IQueryable<TEntity> Get(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            var query = this._repository.Queryable;
            if (func == null) return query;
            query = func.Invoke(query);
            return query;
        }

        public virtual IList<TEntity> GetEntities(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return this.Get(query=>query.Where(expression)).ToList();
        }

        public virtual IList<TEntity> GetEntitiesByIds<TId>(IEnumerable<TId> ids)
        {
            if (ids == null || ids.Count() <= 0) return default(IList<TEntity>);

            var newIds = ids.Where(x => !x.Equals(default(TId))).Select(x => (object)x);

            return _repository.GetEntities(newIds);
        }

        public virtual TEntity GetEntity(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return this.Get(query => query.Where(expression)).FirstOrDefault();
        }

        public virtual TEntity GetEntityById<TId>(TId id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            return _repository.GetEntity(id);
        }

        public virtual bool Insert(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _repository.Insert(entity);
            return _repository.SaveChanges()==1;
        }

        public virtual bool Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            if (entities.Count() <= 0) return false;

            _repository.Insert(entities);
            return _repository.SaveChanges() > 0;
        }

        public virtual bool Remove(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _repository.Remove(entity);
            return _repository.SaveChanges() == 1;
        }

        public virtual bool Remove(IEnumerable<TEntity> entities)
        {
             _repository.Remove(entities);
            return _repository.SaveChanges() > 0;
        }

        public virtual bool Update(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _repository.Update(entity);
            return _repository.SaveChanges() == 1;
        }

        public virtual bool Update(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            if (entities.Count() <= 0) return false;

             _repository.Update(entities);
            return _repository.SaveChanges()>0;
        }
    }
}
