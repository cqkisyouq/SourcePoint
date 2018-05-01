using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SourcePoint.Infrastructure.Extensions.ODataExtension.ExpressionVisitors
{
    public class ConstantVisitor : ExpressionVisitor
    {
        public StringBuilder ValueBuilder { get; set; } = new StringBuilder();
        protected override Expression VisitConstant(ConstantExpression node)
        {
            VisitorStringValue(node);
            return base.VisitConstant(node);
        }

        private string VisitorStringValue(ConstantExpression constantExpression)
        {
            string strValue = string.Empty;
            if (constantExpression == null || constantExpression.Value == null) return strValue;
            var valueType = constantExpression.Value.GetType();
            var isOdataParamter = valueType.BaseType.Name.Equals("LinqParameterContainer", StringComparison.OrdinalIgnoreCase);
            if (isOdataParamter == false) return strValue;

            var value = valueType.GetProperty("Property");

            if (value != null) strValue = value.GetValue(constantExpression.Value).ToString();
            ValueBuilder.Append(strValue);
            return strValue;
        }
    }
}
