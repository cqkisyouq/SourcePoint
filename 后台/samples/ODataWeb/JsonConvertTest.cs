using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.OData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataWeb
{

    public class JsonConvertTest : JsonConverter
    {
        public override bool CanRead => base.CanRead;
        public override bool CanWrite => base.CanWrite;
        public override bool CanConvert(Type objectType)
        {
            return typeof(Delta<>) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var valueType = objectType.GenericTypeArguments[0];
            var item = Activator.CreateInstance(valueType);
            serializer.Populate(reader, item);
            return Activator.CreateInstance(objectType,item);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
