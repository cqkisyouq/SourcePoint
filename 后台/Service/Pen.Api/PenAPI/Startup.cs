using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SourcePoint.Infrastructure.Extensions.SwaggerExtension;
using System.IO;

namespace PenAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.SetBasePath(Directory.GetCurrentDirectory());
            cfgBuilder.AddJsonFile($"appsettings.json", true, true);
            cfgBuilder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
            Configuration = cfgBuilder.Build();
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerDocument(info =>
            {
                info.Title = "地图信息服务";
                info.Version = "v1.0";
                info.Description = "为查询地图数据提供所需要的服务";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseMvc();
            app.UseSwaggerDocument("v1");
        }
    }
}
