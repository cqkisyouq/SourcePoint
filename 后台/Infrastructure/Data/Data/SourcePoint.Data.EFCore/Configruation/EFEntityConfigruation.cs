using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SourcePoint.Data.BaseData.Bases;

namespace SourcePoint.Data.EFCore.Configruation
{
    public abstract class EFEntityConfigruation<TEntity> : BaseEntityConfigruation<TEntity>, IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        public abstract void Configure(EntityTypeBuilder<TEntity> builder);
    }
}
