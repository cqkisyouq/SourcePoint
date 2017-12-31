using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using SourcePoint.Infrastructure.Extensions.SwaggerExtension.Filters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;

namespace SourcePoint.Infrastructure.Extensions.SwaggerExtension
{
    public static class SettingExtension
    {
        static ApplicationEnvironment application = PlatformServices.Default.Application;
        static string version = application.ApplicationVersion;
        /// <summary>
        /// 添加Swagger文档
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="version">版本号</param>
        /// <param name="info">文档信息</param>
        /// <param name="options">文档设置</param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerDocument(this IServiceCollection serviceCollection,Action<Info> info=null, Action<SwaggerGenOptions> options=null)
        {
            serviceCollection.AddSwaggerGen(x =>
            {
                Info settingInfo = new Info() { Title = application.ApplicationName, Version = version, Description = application.RuntimeFramework.FullName };
                
                if (info != null) info(settingInfo);

                x.SwaggerDoc(version, settingInfo);
                if (options != null) options(x);

                x.IncludeXmlComments();
            });

            return serviceCollection;
        }


        public static IServiceCollection AddODataSwaggerDocument(this IServiceCollection serviceCollection, Action<Info> info = null, Action<SwaggerGenOptions> options = null)
        {
            serviceCollection.AddSwaggerGen(x =>
            {
                Info settingInfo = new Info() { Title = application.ApplicationName, Version = version, Description = application.RuntimeFramework.FullName };

                if (info != null) info(settingInfo);
                x.DocumentFilter<ODataDocumentFilter>(serviceCollection.BuildServiceProvider());
                x.SwaggerDoc(version, settingInfo);
                if (options != null) options(x);

                x.IncludeXmlComments();
            });

            return serviceCollection;
        }

        /// <summary>
        /// 使用Swagger文档
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="apiName">下拉框显示名称</param>
        /// <param name="versionString">版本号 获取文档</param>
        /// <param name="uiOptions">UI显示配置</param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerDocument(this IApplicationBuilder applicationBuilder,string versionString=null,string apiName=null,Action<SwaggerUIOptions> uiOptions=null)
        {
            if(string.IsNullOrEmpty(versionString)==false) version = versionString; 

            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint($"/Swagger/{version}/swagger.json",string.IsNullOrEmpty(apiName)?application.ApplicationName : apiName);
                if (uiOptions != null) uiOptions(setup);
            });
          
            return applicationBuilder;
        }

        /// <summary>
        /// 加载程序注释文件
        /// </summary>
        /// <param name="options"></param>
        public static void IncludeXmlComments(this SwaggerGenOptions options)
        {
            var app = PlatformServices.Default.Application;
            string[] xmlFiles = Directory.GetFiles(app.ApplicationBasePath, "*.xml");
            foreach (var file in xmlFiles)
            {
                options.IncludeXmlComments(file);
            }
        }
    }
}
