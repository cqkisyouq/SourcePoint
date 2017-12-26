using CacheManager.Core;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace SourcePoint.Service.ApiGateway
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("configuration.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Action<ConfigurationBuilderCachePart> settings = (x) =>
            {
                x.WithMicrosoftLogging(log =>
                {
                    log.AddConsole(Microsoft.Extensions.Logging.LogLevel.Debug);
                })
                .WithDictionaryHandle();
            };

            //需要把 Program里把 IWebHostBuilder注入进来  不然后面会报错
            //var whb = services.First(x => x.ServiceType == typeof(IWebHostBuilder));
            
            //用于网关身份验证 
            //services.AddAuthentication()
            //   .AddJwtBearer("TestKey", x =>
            //   {
            //       x.Authority = "test";
            //       x.Audience = "test";
            //   });

            //InitialTestConsulService(); 用于测试 负载均橫使用的Consul服务信息
            services.AddOcelot(Configuration, settings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app
            , IHostingEnvironment env
            ,ILoggerFactory loggerFactory)
        {

            //使用格式  http://localhost:8088/platform/Navigations
            //看configuration.json中的配置对应上

            app.UseStaticFiles();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            app.UseOcelot().Wait();
        }

        /// <summary>
        /// 进行负载初始化测试  使用Consul 需要先下载Consul  并启动
        /// </summary>
        private void InitialTestConsulService()
        {
            ConsulClient consul = new ConsulClient(x => x.Address = new Uri("http://localhost:8500"));

            // ID 相同只能注册 一个服务   ID是唯一的。
            AgentServiceRegistration model = new AgentServiceRegistration()
            {
                Name = "newapp",//用于配置文件 中的 ServiceName
                ID = "newapp",
                Port = 8026,
                Address = "localhost" //Api服务 真实地址
            };
            consul.Agent.ServiceRegister(model).GetAwaiter().GetResult();
            
            // 在这里注册二个测试一下 负载的效果

            model.Port = 8025;
            model.ID = "newapp2";
            consul.Agent.ServiceRegister(model).GetAwaiter().GetResult();
        }
    }
}
