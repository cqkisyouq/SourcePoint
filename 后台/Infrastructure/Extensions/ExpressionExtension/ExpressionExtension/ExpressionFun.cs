using System;
using System.Linq;
using System.Linq.Expressions;

namespace SourcePoint.Infrastructure.Extensions.ExpressionExtension
{
    /// <summary>
    /// 扩展 Expression
    /// </summary>
    public static class ExpressionFun
    {
        /// <summary>
        /// 扩展 and的运算符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
        /// <summary>
        /// 扩展or的运算符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);

            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// 扩展 If-Else 操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="isTrue">条件</param>
        /// <param name="trueExpression">true 逻辑</param>
        /// <param name="falseExpression">false 逻辑</param>
        /// <returns></returns>
        public static Expression<Func<T,bool>> WhereIf<T>(this Expression<Func<T,bool>> expression, Expression<Func<T, bool>> isTrue, Expression<Func<T,bool>> trueExpression,Expression<Func<T,bool>> falseExpression)
        {
            var parms = expression.Parameters.ToArray();

            var condition = BoolExpression(isTrue, parms);
            var trueResult = BoolExpression(trueExpression, parms);
            var falseResult = BoolExpression(falseExpression, parms);
            
            var result = Expression.Lambda<Func<T,bool>>(Expression.Condition(
                condition,
                trueResult,
                falseResult
                ),parms);

            return result;
        }
        

        public static InvocationExpression BoolExpression<T>(Expression<Func<T,bool>> expression,params Expression[] value)
        {
            return Expression.Invoke(expression, value);
        }

        /// <summary>
        /// 扩展 If-Else 操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="isTrue">条件</param>
        /// <param name="trueExpression">true 逻辑</param>
        /// <param name="falseExpression">false 逻辑</param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> expression,bool condition,Expression<Func<T,bool>> trueExpression)
        {
            return condition ? expression.Where(trueExpression) : expression;
        }
    }
}
