using EFDataAuth.Test.Domain.Data;
using EFDataAuth.Test.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SourcePoint.Data.EFCore.DataContext;
using System;

namespace EFDataAuth.Test.Domain
{
    public class MyTestDbContext: EFBaseDataContext
    {
        public MyTestDbContext(DbContextOptions<MyTestDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }
        public DbSet<Users> Users { get; set; }
        public  DbSet<Adress> Adress { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
