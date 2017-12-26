using SourcePoint.Data.BaseData.Bases;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using SourcePoint.Data.EFCore.Interface;
using Microsoft.EntityFrameworkCore;

namespace SourcePoint.Data.EFCore.Repository
{
    public class EFRepository<TEntity> : BaseRepository<TEntity> where TEntity : BaseEntity
    {
        public EFRepository(IEFDataContext dataContext)
        {
            db = dataContext;
            Entity = db.Set<TEntity>();
        }

        protected IEFDataContext db { get; }
        protected DbSet<TEntity> Entity { get; }

        public override IQueryable<TEntity> Queryable => Entity;

        public override IQueryable<TEntity> QueryableNoTracking => Queryable.AsNoTracking();


        public override int Delete(TEntity entity)
        {
            entity.Deleted = true;
            var dbEntity = db.Entry(entity);

            if (dbEntity.State == EntityState.Detached)
            {
                dbEntity.State = EntityState.Modified;
            }
            return 1;
        }

        public override int Delete(IEnumerable<TEntity> entities)
        {
            entities.AsParallel().ForAll(entity =>
            {
                Delete(entity);
            });

            return entities.Count();

        }

        public override IList<TEntity> GetEntities(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null) return Queryable.ToList();

            return Queryable.Where(expression).ToList();
        }

        public override TEntity GetEntity(object id)
        {
            if (id == null || !Guid.TryParse(id.ToString(), out Guid gid)) return default(TEntity);

            return Queryable.FirstOrDefault(x => x.Id == gid);
        }

        public override IList<TEntity> GetEntities(IEnumerable<object> ids)
        {
            if (ids == null || ids.Count() <= 0) return default(IList<TEntity>);

            return Queryable.Where(x => ids.Contains(x.Id)).ToList();
        }

        public override TEntity Insert(TEntity entity)
        {
            entity.CreateTime = DateTime.Now;
            entity.UpdateTime = DateTime.Now;
            Entity.Add(entity);

            return entity;
        }

        public override IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                Insert(item);
            }

            return entities;
        }

        public override int Remove(TEntity entity)
        {
            Entity.Remove(entity);
            return 1;
        }

        public override int Remove(IEnumerable<TEntity> entities)
        {
            Entity.RemoveRange(entities);
            return entities.Count();
        }

        public override TEntity Update(TEntity entity)
        {
            Entity.Update(entity);
            return entity;
        }

        public override IEnumerable<TEntity> Update(IEnumerable<TEntity> entities)
        {
            Entity.UpdateRange(entities);
            return entities;
        }

        public override int SaveChanges()
        {
            return db.SaveChanges();
        }

    }
}
