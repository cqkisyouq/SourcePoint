using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;
using System.Reflection;

namespace MapPen.Data.Extensions
{
    public interface IDbEntityConfiguration
    {
        void Configure(EntityTypeBuilder entity);

        Type EntityType { get; }
    }

    public abstract class DbEntityConfiguration<TEntity> : IDbEntityConfiguration
        where TEntity : class
    {
        public abstract void Configure(EntityTypeBuilder<TEntity> entity);

        public void Configure(EntityTypeBuilder entity)
        {
            var model = new Model();
            var entityType = new EntityType(typeof(TEntity), model, ConfigurationSource.Explicit);
            var internalModelBuilder = model.Builder;
            var internalEntityTypeBuilder = new InternalEntityTypeBuilder(entityType, internalModelBuilder);
            var entityTypeBuilder = new EntityTypeBuilder<TEntity>(internalEntityTypeBuilder);
            Configure(entityTypeBuilder);
        }

        public Type EntityType
        {
            get
            {
                return typeof(TEntity);
            }
        }
    }

    public static class ModelBuilderExtension
    {
        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, DbEntityConfiguration<TEntity> entityConfiguration)
          where TEntity : class
        {
            modelBuilder.Entity<TEntity>(entityConfiguration.Configure);
        }

        public static void AddConfigurationFromAssembly(this ModelBuilder modelBuilder, Assembly assembly)
        {
            var types = assembly.GetTypes().Where(x => typeof(IDbEntityConfiguration).IsAssignableFrom(x));
            var instances = types.Select(x => (IDbEntityConfiguration)Activator.CreateInstance(x)).ToList();
            foreach (var instance in instances)
                modelBuilder.Entity(instance.EntityType, instance.Configure);
        }
    }
}
