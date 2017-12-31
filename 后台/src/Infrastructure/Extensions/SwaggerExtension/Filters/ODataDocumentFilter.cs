using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OData.Edm;
using SourcePoint.Infrastructure.Extensions.SwaggerExtension.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace SourcePoint.Infrastructure.Extensions.SwaggerExtension.Filters
{
    /// <summary>
    ///生成 OData 格式API
    /// </summary>
    public class ODataDocumentFilter : IDocumentFilter
    {
        public readonly IServiceProvider provider;
        private List<IOperationFilter> _filters { get; set; } = new List<IOperationFilter>();

        public ODataDocumentFilter(IServiceProvider pro)
        {
            provider = pro;
            LoadXmlDocumnets();
        }



        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            Dictionary<string, PathItem> newpath = new Dictionary<string, PathItem>();

            //foreach (var item in swaggerDoc.Paths)
            //{
            //    newpath.Add(Prefix(FindPre(item.Value)) + item.Key, item.Value);
            //}

            newpath = ODataPath(context.SchemaRegistry);

            foreach (var item in swaggerDoc.Paths)
            {
                newpath.Add(item.Key, item.Value);
            }
            swaggerDoc.Paths = newpath;
            swaggerDoc.Definitions = context.SchemaRegistry.Definitions;
        }

        /// <summary>
        /// 生成 OData Url请求格式
        /// </summary>
        /// <param name="schemaRegistry"></param>
        /// <returns></returns>
        public Dictionary<string, PathItem> ODataPath(ISchemaRegistry schemaRegistry)
        {
            Dictionary<string, PathItem> result = new Dictionary<string, PathItem>();

            var edmItems = provider.GetService<ODataEdmModelManager>();
            if (edmItems == null || edmItems.GetItems.Count <= 0) return result;

            var actions = provider.GetService<IActionDescriptorCollectionProvider>().ActionDescriptors.Items;
            foreach (var edmModelMap in edmItems.GetItems)
            {
                var edmModel = edmModelMap.EdmModel;
                var controllers = actions.OfType<ControllerActionDescriptor>().ToList();
                var removeControllers = AddFunctionOrActionApi(edmModelMap.Key, edmModel, controllers, result, schemaRegistry);

                if (removeControllers.Count > 0) controllers.RemoveAll(action => removeControllers.Contains(action));

                var containers = edmModel.SchemaElements.Where(x => x.SchemaElementKind == EdmSchemaElementKind.EntityContainer).SelectMany(x => ((EdmEntityContainer)x).Elements).Select(x => (EdmEntitySet)x).ToList();

                var controllerItems = controllers.GroupBy(x => x.ControllerName).ToList();
                foreach (var item in controllerItems)
                {
                    AddODataOtherApi(edmModelMap.Key, containers, item.ToList(), result, schemaRegistry);
                }
            }

            return result;
        }

        public List<ControllerActionDescriptor> AddFunctionOrActionApi(string prefix, IEdmModel edmModel, List<ControllerActionDescriptor> controllers, Dictionary<string, PathItem> result, ISchemaRegistry schemaRegistry)
        {
            var removeControllers = new List<ControllerActionDescriptor>();

            foreach (var item in edmModel.SchemaElements.Where(x => x.SchemaElementKind == EdmSchemaElementKind.Action || x.SchemaElementKind == EdmSchemaElementKind.Function))
            {
                var action = controllers.FirstOrDefault(x => x.ActionName == item.Name);
                if (action != null)
                {
                    var paramters = action.Parameters;
                    Operation operation = new Operation()
                    {
                        OperationId = $"{action.Id}",
                        Tags = new List<string>() { action.ControllerName },
                        Parameters = paramters.Select(x => CreateParamters((ControllerParameterDescriptor)x, schemaRegistry)).ToList(),
                        Responses = CreateResponse(action, schemaRegistry)
                    };

                    var path = $"/{prefix}/{action.ControllerName}/{item.Namespace}.{item.Name}";

                    paramters = paramters.Where(x => x.BindingInfo == null || x.BindingInfo.BindingSource != BindingSource.Header && x.BindingInfo.BindingSource != BindingSource.Body).ToList();

                    if (item.SchemaElementKind == EdmSchemaElementKind.Function && paramters.Count > 0)
                    {
                        List<string> paraterms = new List<string>();
                        foreach (var ps in paramters)
                        {
                            var isQuery = ps.BindingInfo != null && ps.BindingInfo.BindingSource == BindingSource.Query;
                            if (isQuery == false) paraterms.Add($"{ps.Name}={ConvertODataParamterName(ps.Name, IsStringParamter(ps.ParameterType))}");
                        }
                        path += "(" + string.Join(",", paraterms) + ")";
                    }

                    if (result.TryGetValue(path, out PathItem pathItem) == false)
                    {
                        result.Add(path, pathItem = new PathItem());
                    }

                    if (item.SchemaElementKind == EdmSchemaElementKind.Action) pathItem.Post = operation;
                    if (item.SchemaElementKind == EdmSchemaElementKind.Function) pathItem.Get = operation;

                    removeControllers.Add(action);
                }
            }

            return removeControllers;

        }

        public void AddODataOtherApi(string prefix, List<EdmEntitySet> containers, List<ControllerActionDescriptor> controllers, Dictionary<string, PathItem> result, ISchemaRegistry schemaRegistry)
        {
            foreach (var item in controllers)
            {
                var method = item.MethodInfo.GetCustomAttribute<HttpMethodAttribute>();
                var paramters = item.Parameters;

                Operation operation = new Operation()
                {
                    OperationId = $"{item.Id}",
                    Tags = new List<string>() { item.ControllerName },
                    Parameters = item.Parameters.Select(x => CreateParamters((ControllerParameterDescriptor)x, schemaRegistry)).ToList(),
                    Responses = CreateResponse(item, schemaRegistry)
                };

                if (item.MethodInfo.CustomAttributes.Any(x => x.AttributeType.Name == "EnableQueryAttribute"))
                {
                    AddQueryOptionParametersForEntitySet(operation.Parameters);
                }

                var controller = containers.FirstOrDefault(x => x.Name == item.ControllerName);
                if (controller == null) continue;

                var keyType = EdmCoreModel.Instance.GetPrimitiveType(controller.EntityType().DeclaredKey.First().Type.PrimitiveKind());

                var isKey = paramters.FirstOrDefault(x => x.ParameterType.Name == keyType.Name);

                var path = $"/{prefix}/{item.ControllerName}";

                if (isKey != null) path += $"({ConvertODataParamterName(isKey.Name, IsStringParamter(keyType.Name))})";

                bool isProperty = controller.EntityType().Properties().Any(x => string.Equals($"get{x.Name}", item.ActionName, StringComparison.OrdinalIgnoreCase));

                if (paramters.Count > 0 && isKey != null && isProperty) path += $"/{item.ActionName.Substring(3)}";

                paramters = paramters.Where(x => x.BindingInfo == null || x.BindingInfo.BindingSource != BindingSource.Header && x.BindingInfo.BindingSource != BindingSource.Body).ToList();

                if (paramters.Count > 0)
                {
                    List<string> paraterms = new List<string>();

                    foreach (ControllerParameterDescriptor ps in paramters)
                    {
                        var isbosy = method != null && method.HttpMethods.Any(x => postMethod.Contains(x.ToLower()));
                        var isQuery = ps.BindingInfo != null && ps.BindingInfo.BindingSource == BindingSource.Query;

                        if (ps != isKey && isbosy == false && isQuery == false)
                        {
                            paraterms.Add($"{ps.Name}={ConvertODataParamterName(ps.Name, IsStringParamter(ps.ParameterType))}");
                        }
                    }

                    if (paraterms.Count > 0) path += "(" + string.Join(",", paraterms) + ")";
                }
                if (result.TryGetValue(path, out PathItem pathItem) == false)
                {
                    result.Add(path, pathItem = new PathItem());
                }

                SettingMethod(pathItem, method, operation);

                OperationFilterContext operationFilterContext = new OperationFilterContext(new Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription()
                {
                    ActionDescriptor = item
                }, schemaRegistry);

                ApplyFilters(operation, operationFilterContext);
            }
        }

        public static readonly List<string> postMethod = new List<string>()
        {
             "post","put","patch"
        };

        public void SettingMethod(PathItem pathItem, HttpMethodAttribute method, Operation operation)
        {
            if (method == null)
            {
                if (pathItem.Get == null) pathItem.Get = operation;
                return;
            }

            switch (method.HttpMethods.First().ToLower())
            {
                case "get":
                    if (pathItem.Get == null) pathItem.Get = operation;
                    break;
                case "put":
                    if (pathItem.Put == null) pathItem.Put = operation;
                    break;
                case "post":
                    if (pathItem.Post == null) pathItem.Post = operation;
                    break;
                case "delete":
                    if (pathItem.Delete == null) pathItem.Delete = operation;
                    break;
                case "patch":
                case "merge":
                    if (pathItem.Patch == null) pathItem.Patch = operation;
                    break;
                default:
                    throw new InvalidOperationException($"HttpMethod {method.Name} is not supported.");
            }
        }

        /// <summary>
        /// 创建接口参数
        /// </summary>
        /// <param name="pams"></param>
        /// <param name="schemaRegistry"></param>
        /// <returns></returns>
        public IParameter CreateParamters(ControllerParameterDescriptor pams, ISchemaRegistry schemaRegistry)
        {

            var location = GetParameterLocation(pams);

            var schema = (pams.ParameterType == null) ? null : schemaRegistry.GetOrRegister(pams.ParameterType);

            if (location == "body")
            {
                return new BodyParameter
                {
                    Name = pams.Name,
                    Description = pams.Name,
                    Schema = schema
                };
            }

            if (location != "header" && location != "query") location = "path";

            var nonBodyParam = new NonBodyParameter
            {
                Name = pams.Name,
                In = location,
                Required = (location == "path"),
            };

            if (schema == null)
                nonBodyParam.Type = "string";
            else nonBodyParam.PopulateFrom(schema);

            if (nonBodyParam.Type == "array")
                nonBodyParam.CollectionFormat = "multi";

            return nonBodyParam;
        }

        /// <summary>
        /// 创建接口返回数据格式
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <param name="schemaRegistry"></param>
        /// <returns></returns>
        public Dictionary<string, Response> CreateResponse(ControllerActionDescriptor actionDescriptor, ISchemaRegistry schemaRegistry)
        {
            var result = new Dictionary<string, Response>();
            var responseType = actionDescriptor.FilterDescriptors
                .Where(x => x.Filter is IFilterMetadata)
                .Select(x => x.Filter).OfType<ProducesResponseTypeAttribute>()
                .DefaultIfEmpty(new ProducesResponseTypeAttribute(200));

            foreach (var item in responseType)
            {
                var statusCode = item.StatusCode.ToString();
                var model = new Response();

                model.Description = ResponseDescriptionMap.FirstOrDefault(x => Regex.IsMatch(statusCode, x.Key)).Value;
                model.Schema = (item.Type != null && item.Type != typeof(void))
                    ? schemaRegistry.GetOrRegister(item.Type)
                    : null;

                result.Add(statusCode, model);
            }
            return result;
        }

        #region private

        /// <summary>
        /// 转换成OData参数格式
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="isString">true:string类型参数 false:其它类型</param>
        /// <returns></returns>
        private string ConvertODataParamterName(string name, bool isString)
        {
            return isString ? $"'{{{name}}}'" : $"{{{name}}}";
        }

        /// <summary>
        /// 判断是否是String类型的参数
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private bool IsStringParamter(Type type)
        {
            return IsStringParamter(type.Name);
        }

        /// <summary>
        ///  判断是否是String类型的参数
        /// </summary>
        /// <param name="type">类型名称</param>
        /// <returns></returns>
        private bool IsStringParamter(string type)
        {
            return string.Equals(type, "string", StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// 加载 xml文档
        /// </summary>
        private void LoadXmlDocumnets()
        {
            var app = PlatformServices.Default.Application;
            string[] xmlFiles = Directory.GetFiles(app.ApplicationBasePath, "*.xml");
            foreach (var file in xmlFiles)
            {
                var xmlPath = new XPathDocument(file);
                _filters.Add(new XmlCommentsOperationFilter(xmlPath));
            }
        }

        /// <summary>
        /// 生成 方法注释
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="operationFilterContext"></param>
        private void ApplyFilters(Operation operation, OperationFilterContext operationFilterContext)
        {
            _filters.ForEach(item =>
            {
                item.Apply(operation, operationFilterContext);
            });
        }

        /// <summary>
        /// 获取参数描述类型
        /// </summary>
        /// <param name="paramDescription"></param>
        /// <returns></returns>
        private string GetParameterLocation(ParameterDescriptor paramDescription)
        {
            if (paramDescription.BindingInfo == null) return "path";

            if (paramDescription.BindingInfo.BindingSource == BindingSource.Form)
                return "formData";
            else if (paramDescription.BindingInfo.BindingSource == BindingSource.Body)
                return "body";
            else if (paramDescription.BindingInfo.BindingSource == BindingSource.Header)
                return "header";
            else if (paramDescription.BindingInfo.BindingSource == BindingSource.Path)
                return "path";
            else if (paramDescription.BindingInfo.BindingSource == BindingSource.Query)
                return "query";

            return "query";
        }

        /// <summary>
        /// 响应描述
        /// </summary>
        private static readonly Dictionary<string, string> ResponseDescriptionMap = new Dictionary<string, string>
        {
            { "1\\d{2}", "Information" },
            { "2\\d{2}", "Success" },
            { "3\\d{2}", "Redirect" },
            { "400", "Bad Request" },
            { "401", "Unauthorized" },
            { "403", "Forbidden" },
            { "404", "Not Found" },
            { "405", "Method Not Allowed" },
            { "406", "Not Acceptable" },
            { "408", "Request Timeout" },
            { "409", "Conflict" },
            { "4\\d{2}", "Client Error" },
            { "5\\d{2}", "Server Error" }
        };


        /// <summary>
        ///添加 OData 查询方法
        /// </summary>
        /// <param name="parameterList">参数集合</param>
        /// <returns></returns>
        private static IList<IParameter> AddQueryOptionParametersForEntitySet(IList<IParameter> parameterList)
        {
            parameterList.Add(new NonBodyParameter()
            {
                Name = "$filter",
                Type = "string",
                Description = "Filters the results, based on a Boolean condition.",
                In = "query",
                Required = false
            });
            parameterList.Add(new NonBodyParameter()
            {
                Name = "$select",
                Type = "string",
                Description = "Selects which properties to include in the response.",
                In = "query",
                Required = false
            });

            parameterList.Add(new NonBodyParameter()
            {
                Name = "$orderby",
                Type = "string",
                Description = "Sorts the results.",
                In = "query",
                Required = false
            });

            parameterList.Add(new NonBodyParameter()
            {
                Name = "$skip",
                Type = "integer",
                Description = "Skips the first n results.",
                In = "query",
                Required = false
            });

            parameterList.Add(new NonBodyParameter()
            {
                Name = "$top",
                Type = "integer",
                Description = "Returns only the first n results.",
                In = "query",
                Required = false
            });

            parameterList.Add(new NonBodyParameter()
            {
                Name = "$count",
                Type = "boolean",
                Description = "Return Data Count.",
                In = "query",
                Required = false
            });

            parameterList.Add(new NonBodyParameter()
            {
                Name = "$expand",
                Type = "string",
                Description = "selected sub data",
                In = "query",
                Required = false
            });
            return parameterList;
        }

        #endregion
    }
}
