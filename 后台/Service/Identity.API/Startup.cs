using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SourcePoint.Service.Identity.API.Configuration;
using SourcePoint.Service.Identity.API.Validator;
using System;
using System.Linq;
using System.Reflection;

namespace SourcePoint.Service.Identity.API
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
            #region 采用内存数据  identityService4
            //var clients = new List<Client>() {
            //    new Client(){
            //        ClientName ="ZhouServer",
            //        ClientSecrets=new List<Secret>(){ new Secret("hsL7J6nACUol1916h1yiPygPhxnGFiwA", null) },
            //        ClientId="zhou.test"
            //    },
            //};
            //var IdentityResources = new List<IdentityResource>()
            //{
            //    new IdentityResource(){
            //        DisplayName="ceshi API",
            //        Name="ceshi",
            //        Enabled=true
            //    },
            //};

            //services.AddIdentityServer()
            //    .AddInMemoryIdentityResources(IdentityResources)
            //    .AddInMemoryClients(clients);


            #endregion


            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var conString = Configuration.GetConnectionString("Default");
            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddSecretParser<JwtBearerClientAssertionSecretParser>()
                    .AddSecretValidator<PrivateKeyJwtSecretValidator>()
                    .AddConfigurationStore(store =>
                    {
                        store.ConfigureDbContext = options =>
                        options.UseSqlServer(conString
                            , optionAction => optionAction.MigrationsAssembly(migrationsAssembly)
                        );
                    })
                    .AddOperationalStore(store =>
                    {
                        store.ConfigureDbContext = options =>
                        options.UseSqlServer(conString
                            , optionAction => optionAction.MigrationsAssembly(migrationsAssembly)
                        );
                    });

            services.AddTransient<IResourceOwnerPasswordValidator, PenResourcePassword>();

            //services.AddMvc();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIdentityServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseMvc();

            //在进行数据迁移的时候把这句注了 用来初始化数据的
            //Add-Migration -c PersistedGrantDbContext name  这里因为有多个DbContext 需要指定对谁做迁移
            //Add-Migration -c ConfigurationDbContext
            //update-database -c PersistedGrantDbContext  使用的时候跟上面一样 在多配置下需要指定使用谁的
            //InitIdentityData(app.ApplicationServices);
        }

        private void InitIdentityData(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
               // serviceScope.ServiceProvider.GetService<ConfigurationDbContext>().Database.Migrate();
               // serviceScope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();
                EnsureSeedData(serviceScope.ServiceProvider.GetService<ConfigurationDbContext>());
            }
        }

        private void EnsureSeedData(ConfigurationDbContext configurationDbContext)
        {
            var isInit = false;
            if (configurationDbContext.Clients.Any() == false)
            {
                ClientsConfig.GetClients().ForEach(item =>
                {
                    configurationDbContext.Clients.Add(item.ToEntity());
                });
                isInit = true;
            }

            if (configurationDbContext.ApiResources.Any() == false)
            {
                ResourceConfig.GetApiResource().ForEach(item =>
                {
                    configurationDbContext.ApiResources.Add(item.ToEntity());
                });
                isInit = true;
            }

            if (configurationDbContext.IdentityResources.Any() == false)
            {
                ResourceConfig.GetIdentityResource().ForEach(item =>
                {
                    configurationDbContext.IdentityResources.Add(item.ToEntity());
                });
                isInit = true;
            }

            if(isInit) configurationDbContext.SaveChanges();
        }
    }
}
