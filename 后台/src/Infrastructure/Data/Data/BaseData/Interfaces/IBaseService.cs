using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SourcePoint.Data.BaseData.Interface
{
    public interface IBaseService { }

    public interface IBaseService<TEntity>:IBaseService where TEntity : class
    {
        /// <summary>
        /// 根据条件判断是否存在
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> expression);
        /// <summary>
        ///通过 Iqueryable 形式查询实体
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        IQueryable<TEntity> Get(Func<IQueryable<TEntity>, IQueryable<TEntity>> func=null);
        /// <summary>
        /// 根据ID 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetEntityById<TId>(TId id,Func<IQueryable<TEntity>,IQueryable<TEntity>> func=null);

        /// <summary>
        /// 根据 id 获取实体集
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IList<TEntity> GetEntitiesByIds<TId>(IEnumerable<TId> ids);
        /// <summary>
        /// 根据条件获取实体集合
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IList<TEntity> GetEntities(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        TEntity GetEntity(Expression<Func<TEntity, bool>> expression);
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(TEntity entity);
        /// <summary>
        /// 批量更新实体数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool Update(IEnumerable<TEntity> entities);
        /// <summary>
        /// 添加一个新的实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Insert(TEntity entity);
        /// <summary>
        /// 指添加新实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool Insert(IEnumerable<TEntity> entities);
        /// <summary>
        /// 删除一个实体 软删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete(TEntity entity);
        /// <summary>
        /// 指删除实体集 软删除
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool Delete(IEnumerable<TEntity> entities);
        /// <summary>
        /// 删除一个实体数据 物理删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Remove(TEntity entity);
        /// <summary>
        /// 删除实体集 物理删除
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool Remove(IEnumerable<TEntity> entities);

    }
}
