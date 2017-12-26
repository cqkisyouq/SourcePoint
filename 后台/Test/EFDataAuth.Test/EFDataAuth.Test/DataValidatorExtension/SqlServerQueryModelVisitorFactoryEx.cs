using Microsoft.EntityFrameworkCore.Query;

namespace EFDataAuth.Test.DataValidatorExtension
{
    public class SqlServerQueryModelVisitorFactoryEx : RelationalQueryModelVisitorFactory
    {
        public SqlServerQueryModelVisitorFactoryEx(EntityQueryModelVisitorDependencies dependencies
            , RelationalQueryModelVisitorDependencies relationalDependencies)
            : base(dependencies, relationalDependencies)
        {
        }
        public override EntityQueryModelVisitor Create(QueryCompilationContext queryCompilationContext, EntityQueryModelVisitor parentEntityQueryModelVisitor)
        {
            return new RelationalQueryModelVisitorEx(Dependencies,
                RelationalDependencies
                , (RelationalQueryCompilationContext)queryCompilationContext
                , (RelationalQueryModelVisitor)parentEntityQueryModelVisitor);
        }
    }
}
