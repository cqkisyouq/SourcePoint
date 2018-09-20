using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        private static string IndexPath = "./wwwroot/index.html";
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {

            #region 支持静态文件 设置网站默认打开 index.html 404重定向到 index.html
            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            // defaultFilesOptions.RequestPath = "/wwwroot/dist";
            defaultFilesOptions.DefaultFileNames.Clear();
            //当访问网站域名时  默认请求下面指定的文件
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();

            //对 400 - 499 状态处理 这里为了支持vue前端只处理 404 状态
            app.UseStatusCodePages(async status =>
            {
                if (status.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    if (File.Exists(IndexPath))
                    {
                        var context = File.ReadAllText(IndexPath);
                        status.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                        await status.HttpContext.Response.WriteAsync(context);
                        return;
                    }
                }
                await status.Next(status.HttpContext);
            });

            #endregion

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwaggerDocument("v1");
        }
    }
}
