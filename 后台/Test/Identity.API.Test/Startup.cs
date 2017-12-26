using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;

namespace Identity.API.Test
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
            #region 跨域
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            #endregion

            //ApiResource 所拥的 Scope 一定要跟 Client 有的Scope 对应上
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(option=> {
                    //这里是在对 ApiResource 进行验证  就是在方法上加 [Authorize] 会进行这个验证
                    option.Authority = "http://localhost:8010";
                    option.ApiName = "ApiInfo";  //ApiResource 的Name
                    option.ApiSecret = "passwordq123q"; //ApiResource 的 ApiSecrets
                    option.RequireHttpsMetadata = false;
                });
            
            //AuthenticationBuilder builder = new AuthenticationBuilder(services);
            //builder.AddIdentityServerAuthentication(opertion => {
            //    opertion.Authority = "http://139.217.11.253:8010";
            //    opertion.ApiName = "api1";
            //    opertion.ApiSecret = "secret";
            //    opertion.RequireHttpsMetadata = false;
            //});
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //配置identityServer授权
            app.UseAuthentication();

            //跨域访问
            app.UseCors("default");
            app.UseMvc();
        }
    }
}
