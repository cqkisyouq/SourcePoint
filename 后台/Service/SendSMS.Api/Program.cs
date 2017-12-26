using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SourcePoint.Service.SendSMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
                new WebHostBuilder()
                .UseStartup<Startup>()
                .Build();
    }
}
