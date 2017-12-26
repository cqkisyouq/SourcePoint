using EFDataAuth.Test.Domain.Data;
using EFDataAuth.Test.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EFDataAuth.Test.Domain
{
    public class MyTestDbContext:DbContext, IHttpDbContext
    {
        public IHttpContextAccessor httpContextAccessor { get; set; }
        public MyTestDbContext(DbContextOptions<MyTestDbContext> dbContextOptions, IHttpContextAccessor httpContext)
            : base(dbContextOptions)
        {
            httpContextAccessor = httpContext;
        }
        public DbSet<Users> Users { get; set; }
        public  DbSet<Adress> Adress { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
