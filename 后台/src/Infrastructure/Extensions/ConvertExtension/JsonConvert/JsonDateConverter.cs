using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace SourcePoint.Infrastructure.Extensions.ConvertExtension.JsonConvert
{
    public class JsonDateConverter : DateTimeConverterBase
    {
        private HttpContext httpContext;
        public JsonDateConverter(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null) throw new Exception("httpContext没有注入成功！");
            httpContext = httpContextAccessor.HttpContext;
        }
        public override bool CanConvert(Type objectType)
        {
            return typeof(DateTime) == objectType || typeof(DateTime?) == objectType;
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);

            if (httpContext != null && httpContext.Request.Headers.Any(n => n.Key.ToLower() == "x-timezoneutc"))
            {
                var xtimeZone = httpContext.Request.Headers["x-timezoneutc"];
                double timeZone = 0;
                double.TryParse(xtimeZone.ToString(), out timeZone);

                if (long.TryParse((string)token, out long timestamp))
                {
                    if (timestamp > 9999999999)
                        return new DateTime(1970, 1, 1).ToLocalTime().AddMilliseconds(timestamp).AddMinutes(-timeZone);
                    else
                        return new DateTime(1970, 1, 1).ToLocalTime().AddSeconds(timestamp).AddMinutes(-timeZone);
                }
                else
                    return Convert.ToDateTime(token.ToString()).AddMinutes(-timeZone);
            }
            if (token.Type == JTokenType.Integer)
            {
                if ((long)token > 9999999999)
                    return new DateTime(1970, 1, 1).ToLocalTime().AddMilliseconds((long)token);
                else
                    return new DateTime(1970, 1, 1).ToLocalTime().AddSeconds((long)token);
            }
            else
                return Convert.ToDateTime(token.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (httpContext != null && httpContext.Request.Headers.Any(n => n.Key.ToLower() == "x-timezoneutc"))
            {
                var xtimeZone = httpContext.Request.Headers["x-timezoneutc"];
                double timeZone = 0;
                double.TryParse(xtimeZone.ToString(), out timeZone);

                writer.WriteValue(DateTime.Parse(((DateTime)value).AddMinutes(timeZone).ToString("yyyy-MM-dd HH:mm:ss")));
            }
            else
            {
                writer.WriteValue(value);
            }
            writer.Flush();
        }
    }
}
