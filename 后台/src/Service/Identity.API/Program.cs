using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
namespace SourcePoint.Service.Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
                 WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseUrls("http://*:8010")
                .Build();
    }
}
