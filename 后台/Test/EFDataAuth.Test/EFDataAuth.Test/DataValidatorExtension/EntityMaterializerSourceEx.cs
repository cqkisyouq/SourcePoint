using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EFDataAuth.Test.DataValidatorExtension
{
    public class EntityMaterializerSourceEx : EntityMaterializerSource
    {
        private static readonly MethodInfo _tryReadValueMethodCustomer
           = typeof(EntityMaterializerSourceEx).GetTypeInfo()
               .GetDeclaredMethod(nameof(TryReadValueCustomer));

        public override Expression CreateReadValueExpression(Expression valueBuffer, Type type, int index, IProperty property = null)
        {

            var call = Expression.Call(
                _tryReadValueMethodCustomer.MakeGenericMethod(type),
                valueBuffer,
                Expression.Constant(index),
                Expression.Constant(property, typeof(IPropertyBase)));

            return call;
        }

        /// <summary>
        /// 把数据库中的数据 转换成实体数据
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="valueBuffer"></param>
        /// <param name="index"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private static TValue TryReadValueCustomer<TValue>(
          ValueBuffer valueBuffer,
          int index,
          IPropertyBase property = null)
        {
            object untypedValue = null;

            try
            {

                if (index == -999) return default(TValue);

                untypedValue = valueBuffer[index];

                return (TValue)untypedValue;
            }
            catch (Exception e)
            {
                //ThrowReadValueException<TValue>(e, untypedValue, property);
            }

            return default(TValue);
        }
    }
}
