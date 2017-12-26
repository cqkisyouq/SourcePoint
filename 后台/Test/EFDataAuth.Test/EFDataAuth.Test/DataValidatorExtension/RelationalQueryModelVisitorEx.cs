using Microsoft.EntityFrameworkCore.Query;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using System.Linq;
using System.Linq.Expressions;

namespace EFDataAuth.Test.DataValidatorExtension
{
    public class RelationalQueryModelVisitorEx : RelationalQueryModelVisitor
    {
        public RelationalQueryModelVisitorEx(EntityQueryModelVisitorDependencies dependencies
            , RelationalQueryModelVisitorDependencies relationalDependencies
            , RelationalQueryCompilationContext queryCompilationContext
            , RelationalQueryModelVisitor parentQueryModelVisitor)
            : base(dependencies, relationalDependencies, queryCompilationContext, parentQueryModelVisitor)
        {
        }

        public override void VisitQueryModel(QueryModel queryModel)
        {
            //todo 在这里进行 行级数据过滤  需要循环bodyClauses 解析Body  循环Body里的数据重复解析
            
            var type = queryModel.SelectClause.Selector;
            var joins = queryModel.BodyClauses.Where(x => x is JoinClause).Select(x => (JoinClause)x).ToList();
            var bodys = queryModel.BodyClauses.Where(x => x is WhereClause).Select(x => (WhereClause)x).ToList();
            if (bodys.Count > 0) bodys[0].Predicate = Expression.AndAlso(bodys[0].Predicate, Expression.MakeBinary(ExpressionType.Equal, Expression.Constant(1), Expression.Constant(1)));
            
            base.VisitQueryModel(queryModel);
        }

    }
}
