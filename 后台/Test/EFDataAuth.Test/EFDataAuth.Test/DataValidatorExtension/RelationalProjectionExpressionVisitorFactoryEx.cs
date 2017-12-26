using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFDataAuth.Test.DataValidatorExtension
{
    public class RelationalProjectionExpressionVisitorFactoryEx : RelationalProjectionExpressionVisitorFactory
    {
        //private readonly ISqlTranslatingExpressionVisitorFactory _sqlTranslatingExpressionVisitorFactory;
        //private readonly IEntityMaterializerSource _entityMaterializerSource;
        private readonly RelationalProjectionExpressionVisitorDependencies _Dependencies;

        public RelationalProjectionExpressionVisitorFactoryEx(RelationalProjectionExpressionVisitorDependencies dependencies) : base(dependencies)
        {
            _Dependencies = dependencies;
        }
        
        public override ExpressionVisitor Create(EntityQueryModelVisitor entityQueryModelVisitor, IQuerySource querySource)
        {
            return new RelationalProjectionExpressionVisitorEx(Dependencies, (RelationalQueryModelVisitor)entityQueryModelVisitor, querySource);
        }
    }
}
