using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Builder;
using Microsoft.Extensions.DependencyInjection;
using SourcePoint.Infrastructure.Extensions.ODataExtension.Attributes;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace SourcePoint.Infrastructure.Extensions.ODataExtension.Extensions
{
    public static class ODataModelBuilderExtension
    {
        private static readonly MethodInfo entitySetMethod = typeof(ODataConventionModelBuilder).GetMethod("EntitySet");
        private static readonly MethodInfo entityTypeMethod = typeof(ODataConventionModelBuilder).GetMethod("EntityType");

        public static ODataConventionModelBuilder ModelBuilder(this IServiceCollection serviceCollection, string serviceName)
        {
            var service = serviceCollection.BuildServiceProvider();
            var provider = service.GetRequiredService<IAssemblyProvider>();

            ODataConventionModelBuilder modelbuilder = new ODataConventionModelBuilder(provider);

            if (string.IsNullOrWhiteSpace(serviceName) == false) modelbuilder.Namespace = serviceName;

            var actions = service.GetService<IActionDescriptorCollectionProvider>().ActionDescriptors.Items;
            var controllers = actions.OfType<ControllerActionDescriptor>().ToList();
            var controllerGroup = controllers.GroupBy(n => n.ControllerName);
            var type = typeof(OdataEntityAttribute);
            foreach (var controller in controllerGroup)
            {
                var entityAttribute = controller.First().ControllerTypeInfo.GetCustomAttributes<OdataEntityAttribute>();
                foreach (var item in entityAttribute)
                {
                    var entityType = item.GetEntitysType();
                    var entitySetGenericMethod = entitySetMethod.MakeGenericMethod(entityType);
                    var entitySetConfiguration = entitySetGenericMethod.Invoke(modelbuilder, new object[] { controller.First().ControllerName });

                    foreach (var action in controller)
                    {
                        var actionAttribute = action.MethodInfo.GetCustomAttributes<ODataRequestAttribute>();
                        foreach (var methodAtrribute in actionAttribute)
                        {
                            var entityTypeInvok = entityTypeMethod.MakeGenericMethod(entityType);
                            var actionType = methodAtrribute.HttpMethod == HttpMethod.Get ? "Function" : "Action";
                            var odataFunction = entityTypeInvok.ReturnType.GetProperty("Collection").GetMethod.ReturnType.GetMethod(actionType);
                            var entityTypeValue = entityTypeInvok.Invoke(modelbuilder, null);
                            var collectionProperty = entityTypeValue.GetType().GetProperty("Collection").GetValue(entityTypeValue);

                            var functionConfig = odataFunction.Invoke(collectionProperty, new object[] { action.ActionName });

                            var paramters = action.MethodInfo.GetParameters().Where(x => x.CustomAttributes.Count() <= 0);
                            var paramterMethod = odataFunction.ReturnType.GetMethods().Where(x => x.Name == "Parameter" && x.IsGenericMethod).FirstOrDefault();
                            foreach (var paramter in paramters)
                            {
                                paramterMethod.MakeGenericMethod(paramter.ParameterType).Invoke(functionConfig, new object[] { paramter.Name });
                            }

                            if (methodAtrribute.IsVoid == false)
                            {
                                var returnsMethod = odataFunction.ReturnType.GetMethods().Where(x => x.Name == "Returns" && x.IsGenericMethod).FirstOrDefault();
                                returnsMethod.MakeGenericMethod(methodAtrribute.ReturnType).Invoke(functionConfig, null);
                            }
                        }
                    }
                }
            }
            modelbuilder.EnableLowerCamelCase();

            return modelbuilder;
        }
    }
}
