using Microsoft.Extensions.DependencyInjection;
using SourcePoint.Data.BaseData.Interface;
using SourcePoint.Data.EFCore.Interface;
using SourcePoint.Data.EFCore.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace SourcePoint.Data.EFCore.Extension
{
   public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEFData<TContext>(IServiceCollection service,ServiceLifetime lifetime= ServiceLifetime.Scoped) where TContext : IEFDataContext
        {
            service.Add(new ServiceDescriptor(typeof(IRepository<>), typeof(EFRepository<>), lifetime));
            service.Add(new ServiceDescriptor(typeof(IEFDataContext), typeof(TContext), lifetime));
            return service;
        }
    }
}
