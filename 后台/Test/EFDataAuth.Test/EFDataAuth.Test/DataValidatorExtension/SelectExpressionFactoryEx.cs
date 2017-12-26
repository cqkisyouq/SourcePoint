using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Expressions;

namespace EFDataAuth.Test.DataValidatorExtension
{
    public class SelectExpressionFactoryEx : SelectExpressionFactory
    {
        //private readonly IQuerySqlGeneratorFactory _querySqlGeneratorFactory;
        private readonly SelectExpressionDependencies _Dependecies;

        public SelectExpressionFactoryEx(SelectExpressionDependencies dependencies
            //, ITestService parameterNameGeneratorFactory
            ) : base(dependencies)
        {
            _Dependecies = dependencies;
        }


        public override SelectExpression Create(RelationalQueryCompilationContext queryCompilationContext)
        {

            return new SelectExpressionEx(_Dependecies, queryCompilationContext);
        }
        public override SelectExpression Create(RelationalQueryCompilationContext queryCompilationContext, string alias)
        {
            return base.Create(queryCompilationContext, alias);
        }

    }
}
