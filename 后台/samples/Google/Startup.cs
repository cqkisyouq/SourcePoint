using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SourcePoint.Infrastructure.Extensions.SwaggerExtension;
using SourcePoint.Infrastructure.Authenticator.GoogleAuthenticator.Extension;
using Microsoft.Extensions.Logging;

namespace Google
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddGoogleAuthenticator();
            services.AddSwaggerDocument(info =>
            {
                info.Title = "谷歌验证服务";
                info.Version = "v1.0";
                info.Description = "谷歌验证器提供";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwaggerDocument("v2");
            app.UseMvc();
            
        }
    }
}
