using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SourcePoint.Service.Identity.API.Validator
{
    public class PenResourcePassword : IResourceOwnerPasswordValidator
    {
        private readonly ISystemClock systemClock;
        public PenResourcePassword(ISystemClock system)
        {
            systemClock = system;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //现在这里返的是用户ID   需要反什么  随便
            var userId = context.UserName.GetHashCode().ToString();
            var isAuth = true; //验证结果
            
            if (isAuth)
            {
              //  context.Result = new GrantValidationResult(userId, "password");
                context.Result = new GrantValidationResult(
                       context.UserName,
                       OidcConstants.AuthenticationMethods.Password,
                       systemClock.UtcNow.UtcDateTime,
                       new List<Claim>() //如果 apiResource里没有配置以下属性  下面设置就会不启作用
                       {
                            new Claim(JwtClaimTypes.Name,userId.ToString()), //把用户ID返给Client端
                           new Claim(JwtClaimTypes.Role,"read"),
                           new Claim(JwtClaimTypes.Role,"write"),
                           new Claim(JwtClaimTypes.Role,"search")
                                                                             //new Claim(JwtClaimTypes.Role,"mi"), 这里直接对Client端进行 角色授权
                       }
                   );
            }

            return Task.FromResult(0);
        }
    }
}
