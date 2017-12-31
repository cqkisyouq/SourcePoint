using EFSecondLevelCache.Core;
using EFSecondLevelCache.Core.Contracts;
using MapPen.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenAPI.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DataContext: BaseDbContext
    {
        private readonly IEFCacheServiceProvider _cacheServiceProvider;

        public DataContext(DbContextOptions options, DbConnectionConfig dbConnectionConfig, IEFCacheServiceProvider cacheServiceProvider) :base(options,dbConnectionConfig)
        {
            this._cacheServiceProvider = cacheServiceProvider;
        }

        /// <summary>
        /// 重写保存更改方法，清除缓存
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            this.ChangeTracker.DetectChanges();
            var changedEntityNames = this.GetChangedEntityNames();
            var result = base.SaveChanges();
            _cacheServiceProvider.InvalidateCacheDependencies(changedEntityNames);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ResourceGroupEntity>().HasKey(rg => new { rg.GroupId, rg.ResourceId });
            //modelBuilder.Entity<ResourceGroupEntity>().HasOne(rg => rg.Group).WithMany(group => group.ResourceGroups).HasForeignKey(rg => rg.GroupId);
            //modelBuilder.Entity<ResourceGroupEntity>().HasOne(rg => rg.Resource).WithMany(resource => resource.ResourceGroups).HasForeignKey(rg => rg.ResourceId);
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }


    }
}
