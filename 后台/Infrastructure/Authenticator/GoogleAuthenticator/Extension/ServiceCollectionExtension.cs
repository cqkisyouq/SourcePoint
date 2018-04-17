using Microsoft.Extensions.DependencyInjection;

namespace SourcePoint.Infrastructure.Authenticator.GoogleAuthenticator.Extension
{
    public static  class ServiceCollectionExtension
    {
        public static IServiceCollection AddGoogleAuthenticator(this IServiceCollection service)
        {
            service.AddSingleton<GoogleAuthenticator>();
            return service;
        }
    }
}
