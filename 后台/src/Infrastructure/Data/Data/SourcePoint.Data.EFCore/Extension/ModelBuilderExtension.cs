using Microsoft.EntityFrameworkCore;
using SourcePoint.Data.BaseData.Interface;
using System;
using System.Linq;
using System.Reflection;

namespace SourcePoint.Data.EFCore.Extension
{
    public static class ModelBuilderExtension
    {
        public static readonly MethodInfo Configuration = typeof(ModelBuilderExtension).GetMethod(nameof(ApplyConfiguration));
        
        public static void Register(this ModelBuilder modelBuilder, Type type)
        {
            var types = type.Assembly.GetTypes();
            var _Mappings = types.Where(x => typeof(IEntityConfigruationMap).IsAssignableFrom(x)).ToList();

            _Mappings.ForEach(mapping =>
            {
                var item = Activator.CreateInstance(mapping);
                var entityType = (item as IEntityConfigruationMap).type;
                var method = Configuration.MakeGenericMethod(entityType);
                Configuration.MakeGenericMethod(entityType).Invoke(null, new object[] { modelBuilder, item });
            });
        }

        public static void ApplyConfiguration<T>(ModelBuilder modelBuilder, IEntityTypeConfiguration<T> entityTypeConfiguration) where T : class
        {
            modelBuilder.ApplyConfiguration(entityTypeConfiguration);
        }
    }
}
