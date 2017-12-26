using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace SourcePoint.Service.Identity.API.Validator
{
    public class PenResourcePassword : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //现在这里返的是用户ID   需要反什么  随便
            var userId = context.UserName.GetHashCode().ToString();
            var isAuth = true; //验证结果
            
            if (isAuth)
            {
                context.Result = new GrantValidationResult(userId, "password");
            }

            return Task.FromResult(0);
        }
    }
}
