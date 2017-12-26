using Microsoft.EntityFrameworkCore;
using SourcePoint.Data.EFCore.Extension;
using SourcePoint.Data.EFCore.Interface;

namespace SourcePoint.Data.EFCore.DataContext
{
    public class EFBaseDataContext : DbContext, IEFDataContext
    {
        public EFBaseDataContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Register(GetType());
            base.OnModelCreating(modelBuilder);
        }
    }
}
