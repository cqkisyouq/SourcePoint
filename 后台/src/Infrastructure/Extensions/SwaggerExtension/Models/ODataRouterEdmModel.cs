using Microsoft.OData.Edm;

namespace SourcePoint.Infrastructure.Extensions.SwaggerExtension.Models
{
    public class ODataRouterEdmModel
    {
        public ODataRouterEdmModel() { }
        public ODataRouterEdmModel(string pre, IEdmModel model)
        {
            this.Key = pre;
            this.EdmModel = model;
        }
        public string Key { get; set; }
        public IEdmModel EdmModel { get; set; }
    }
}
