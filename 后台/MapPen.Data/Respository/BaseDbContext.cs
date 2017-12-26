using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MapPen.Data.Extensions;

namespace MapPen.Data
{
    public class BaseDbContext : DbContext, IDbContext
    {
        private readonly DbConnectionConfig _dbConnectionConfig;
        public BaseDbContext(DbContextOptions options, DbConnectionConfig dbConnectionConfig) : base(options)
        {
            //throw new Exception(dbConnectionConfig.ConnectionString);
            _dbConnectionConfig = dbConnectionConfig;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //TODO:DataProvider
            if (_dbConnectionConfig.ProviderType == ProviderType.MsSql)
                optionsBuilder.UseSqlServer(_dbConnectionConfig.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var type = GetType();
            var assembly = type.GetTypeInfo().Assembly;
            //var x = assembly.GetModules();
            modelBuilder.AddConfigurationFromAssembly(assembly); //add mappings

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Detach an entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Detach(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            // base.Detach(entity);
        }
    }
}
