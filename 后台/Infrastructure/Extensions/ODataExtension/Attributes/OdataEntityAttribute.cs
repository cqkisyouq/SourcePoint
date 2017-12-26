using System;

namespace SourcePoint.Infrastructure.Extensions.ODataExtension.Attributes
{
    public class OdataEntityAttribute : Attribute
    {
        private Type type;
        public OdataEntityAttribute(Type type)
        {
            this.type = type;
        }

        public Type GetEntitysType()
        {
            return this.type;
        }
    }
}
