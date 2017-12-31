using SourcePoint.Data.BaseData.Interface;
using System;

namespace SourcePoint.Data.BaseData.Bases
{
    public abstract class BaseEntityConfigruation<TEntity> : IEntityConfigruationMap
    {
        public Type type => typeof(TEntity);
    }
}
