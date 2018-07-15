using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Json.Internal;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SourcePoint.Infrastructure.Extensions.ODataExtension.Extensions;
using SourcePoint.Infrastructure.Extensions.SwaggerExtension;
using SourcePoint.Infrastructure.Extensions.SwaggerExtension.Models;
using System;
using System.Threading.Tasks;

namespace ODataWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ODataRouterEdmModel ODataRouterEdmModel { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MvcJsonOptions>(x => x.SerializerSettings.Converters.Add(new JsonConvertTest()));
           // services.AddMvcCore().AddJsonFormatters(x => x.Converters.Add(new JsonConvertTest()));
            services.AddMvc();
           
            services.AddOData();

            services.AddODataSwaggerDocument(info =>
            {
                info.Title = "OData服务";
                info.Version = "v1.0";
                info.Description = "OData Api 测试";
            });
            
            ODataEdmModelManager oDataEdmModelManager = new ODataEdmModelManager();
            ODataRouterEdmModel = oDataEdmModelManager.AddEdmModel("api", services.ModelBuilder("PlatformService").GetEdmModel());
            services.AddSingleton<ODataEdmModelManager>(oDataEdmModelManager);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseSwaggerDocument("v3");
            app.UseMvc(routes =>
            {
               routes.MapODataRoute(ODataRouterEdmModel.Key, ODataRouterEdmModel.EdmModel);
            });
        }
    }
}
