using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SourcePoint.Data.BaseData.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SourcePoint.Data.EFCore.Interface
{
    public interface IEFDataContext:IDataContext<DatabaseFacade>
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        EntityEntry Entry(object entity);
        int SaveChanges();
    }
}
