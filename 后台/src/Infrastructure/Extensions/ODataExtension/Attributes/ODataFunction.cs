using System;
using System.Net.Http;

namespace SourcePoint.Infrastructure.Extensions.ODataExtension.Attributes
{
    public class ODataFunction : ODataRequestAttribute
    {
        public ODataFunction(Type returnType) : base(returnType)
        {
        }
        public override HttpMethod HttpMethod => HttpMethod.Get;
    }
}
