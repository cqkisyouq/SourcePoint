using EFDataAuth.Test.Validator;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace EFDataAuth.Test.DataValidatorExtension
{
    public class RelationalProjectionExpressionVisitorEx : RelationalProjectionExpressionVisitor
    {
        private IQuerySource _querySource;
        private RelationalQueryModelVisitor _queryModelVisitor;
        private List<Expression> Projection => (List<Expression>)_queryModelVisitor.TryGetQuery(_querySource).Projection;

        public RelationalProjectionExpressionVisitorEx(RelationalProjectionExpressionVisitorDependencies dependencies
            , RelationalQueryModelVisitor queryModelVisitor
            , IQuerySource querySource) : base(dependencies, queryModelVisitor, querySource)
        {
            _querySource = querySource;
            _queryModelVisitor = queryModelVisitor;
        }

        public static T ConvertChange<T, V>(V obj)
        {
            T result = default(T);
            try
            {
                if (typeof(T) != typeof(V)) return result;

                result = (T)Convert.ChangeType(obj, typeof(T));
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public static readonly Type _Type = typeof(RelationalProjectionExpressionVisitorEx);

        public static readonly MethodInfo convertMethod = typeof(RelationalProjectionExpressionVisitorEx).GetTypeInfo().GetMethod(nameof(ConvertChange));



        public override Expression Visit(Expression node)
        {
            
            if (node == null) return node;

            var result = base.Visit(node);

            if (result.NodeType == ExpressionType.Convert)
            {
                var convert = result as UnaryExpression;
                var pas = convert.Operand as MethodCallExpression;
                var parmeter = pas.Arguments.ToArray()[0] as ConstantExpression;
                if (parmeter.Type == typeof(int) &&object.Equals(parmeter.Value,-999))
                {
                    parmeter = Expression.Constant(0);
                    var gentMethod = convertMethod.MakeGenericMethod(convert.Type, parmeter.Type);

                    pas = Expression.Call(gentMethod, parmeter);

                    result = Expression.Convert(pas, convert.Type);
                }
                return result;
            }

            var column = node as MemberExpression;
            if (column != null && TypeValidator.IsValidat(column.Member.Name) == false)
            {
                var select = column.Expression as QuerySourceReferenceExpression;
                var type = select.ReferencedQuerySource.ItemType;
                for (int i = 0; i < Projection.Count; i++)
                {
                    var col = Projection[i] as ColumnExpression;
                    if (col.Name == column.Member.Name)
                    {
                        Projection.RemoveAt(i);
                    }
                }
            }
            
            return result;
        }
    }
}
