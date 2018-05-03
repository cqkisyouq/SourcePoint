using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ODataWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
                 WebHost.CreateDefaultBuilder(args)
                .UseDefaultServiceProvider(x=> { })
                //如果不使用 new WebHostBuilder()  || UseDefaultServiceProvider OData会报错 并且 没有请求日志
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://*:8023")
                .UseStartup<Startup>()
                .Build();
    }
}
