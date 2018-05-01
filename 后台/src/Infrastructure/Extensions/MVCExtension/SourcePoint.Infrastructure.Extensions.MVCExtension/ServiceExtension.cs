
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SourcePoint.Infrastructure.Extensions.MVCExtension
{
    public static class ServiceExtension
    {
        /// <summary>
        /// 添加自己的 razor 页面 搜索
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddRazorProject<T>(this IServiceCollection serviceCollection,params string[] paths)where T: RazorProject
        {
            var project = typeof(RazorProject);
            ServiceDescriptor remove = null;
            foreach (var item in serviceCollection)
            {
               if(item.ServiceType == project)
                {
                    remove = item;
                    break;
                }
            }

            if(remove!=null) serviceCollection.Remove(remove);
            
            serviceCollection.TryAddSingleton(project, typeof(T));

            serviceCollection.AddSingleton<IRazorProjectConfiguration>(x => new RazorProjectConfiguration(paths));

            return serviceCollection;
        }

        public static IServiceCollection AddRazorProject(this IServiceCollection serviceCollection, params string[] paths)
        {
            return AddRazorProject<SourcePointRazorProject>(serviceCollection,paths);
        }

        /// <summary>
        /// 加载目录下所有的dll
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <returns></returns>
        public static IMvcBuilder LoadBasePathAssembly(this IMvcBuilder mvcBuilder)
        {
            string[] xmlFiles = Directory.GetFiles(PlatformServices.Default.Application.ApplicationBasePath, "*.dll");

            //把没有引用程序集的 但是在 bin 目录中 并且实现了 controller 的程序集加载到 mvc管理中 后面会把里面的路由解析出来
            xmlFiles.Select(x => Assembly.LoadFrom(x)).ToList().ForEach(x =>
            {
                if (x.ExportedTypes.Any(d => typeof(Controller).IsAssignableFrom(d)))
                {
                    mvcBuilder.PartManager.ApplicationParts.Add(new AssemblyPart(x));
                }
            });
            return mvcBuilder;
        }
    }
}
