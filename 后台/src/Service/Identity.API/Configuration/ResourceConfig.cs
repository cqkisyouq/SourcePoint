using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourcePoint.Service.Identity.API.Configuration
{
    public class ResourceConfig
    {
        public static List<ApiResource> GetApiResource()
        {
            return new List<ApiResource>()
            {
                new ApiResource()
                {
                    Name="ApiInfo",
                    ApiSecrets={new Secret("passwordq123q".Sha256())},
                    Scopes={
                        new Scope(IdentityServerConstants.StandardScopes.OpenId),
                        new Scope("penApi"),
                        new Scope("apptoken")
                    },//设置允许在  identity 里使用的 名称 
                    UserClaims=new List<string>()
                    {
                        JwtClaimTypes.Role,
                        JwtClaimTypes.Name
                    }
                }
            };
        }

        public static List<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
        
    }
}
