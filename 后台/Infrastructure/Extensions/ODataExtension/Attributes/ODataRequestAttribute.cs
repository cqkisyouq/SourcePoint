using System;
using System.Net.Http;

namespace SourcePoint.Infrastructure.Extensions.ODataExtension.Attributes
{
    public class ODataRequestAttribute:Attribute
    {
        private Type _type=typeof(void);
        private bool isVoid = true;
        
        public ODataRequestAttribute() { }
        public ODataRequestAttribute(Type returnType)
        {
            _type = returnType;
            isVoid = false;
        }

        public virtual HttpMethod HttpMethod { get => HttpMethod.Get; }
        /// <summary>
        /// 返回类型
        /// </summary>
        public Type ReturnType { get => _type; }
        /// <summary>
        /// 是否返回为 void
        /// </summary>
        public bool IsVoid { get => isVoid; }
    }
}
