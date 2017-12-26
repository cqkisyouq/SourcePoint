using EFDataAuth.Test.Validator;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Remotion.Linq.Clauses;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EFDataAuth.Test.DataValidatorExtension
{
    public class SelectExpressionEx : SelectExpression
    {
        public SelectExpressionEx(SelectExpressionDependencies dependencies, RelationalQueryCompilationContext queryCompilationContext)
            : base(dependencies, queryCompilationContext)
        {
        }

        public override int AddToProjection(IProperty property, IQuerySource querySource)
        {
            if (TypeValidator.IsValidat(property.Name) == false)
            {
                return -999;
            }
            return base.AddToProjection(property, querySource);
        }
        public override int AddToProjection(Expression expression, bool resetProjectStar = true)
        {
            if (expression is SelectExpression select)
            {
                var colum = select.Projection.Where(x => x is ColumnExpression).Select(x => (ColumnExpression)x).ToList();
                if (colum.Count == 0)
                {
                    return -999;
                }
            }
            return base.AddToProjection(expression, resetProjectStar);
        }

        public override IReadOnlyList<Expression> Projection => base.Projection;
    }
}
