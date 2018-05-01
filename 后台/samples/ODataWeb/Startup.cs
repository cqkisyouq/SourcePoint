﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SourcePoint.Infrastructure.Extensions.SwaggerExtension;
using SourcePoint.Infrastructure.Extensions.SwaggerExtension.Models;
using SourcePoint.Infrastructure.Extensions.ODataExtension.Extensions;

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
            services.AddMvc();
            services.AddOData();
            services.AddODataSwaggerDocument(info =>
            {
                info.Title = "OData服务";
                info.Version = "v1.0";
                info.Description = "OData Api 测试";
            });

            ODataEdmModelManager oDataEdmModelManager = new ODataEdmModelManager();
            ODataRouterEdmModel = oDataEdmModelManager.AddEdmModel("api", services.ModelBuilder("ODataService").GetEdmModel());
            services.AddSingleton<ODataEdmModelManager>(oDataEdmModelManager);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwaggerDocument("v3");
            app.UseMvc(routes =>
            {
                routes.MapODataRoute(ODataRouterEdmModel.Key, ODataRouterEdmModel.EdmModel);
            });
        }
    }
}
