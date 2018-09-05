using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace SourcePoint.Core.Attribute
{
    /// <summary>
    /// 提供对模块方法进行权限验证的功能 
    /// </summary>
    public class SourcePointAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private static Type _allowType = typeof(IAllowAnonymous);

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var controller = context.ActionDescriptor as ControllerActionDescriptor;

            if (CheckAllowType(controller)) return;
            bool isAuth = context.HttpContext.User.Identity.IsAuthenticated;
            if (isAuth == false)
            {
                SetResponse(context, StatusCodes.Status401Unauthorized);
                return;
            }
            bool isValidat = true;
            var auth = context.HttpContext.RequestServices.GetService<ISourcePointAuth>();
            if (auth != null) isValidat = auth.IsAuth(context.HttpContext, controller);
            if (isValidat == false) SetResponse(context, StatusCodes.Status403Forbidden);
        }

        private void SetResponse(AuthorizationFilterContext context, int code)
        {
            context.HttpContext.Response.StatusCode = code;
            context.Result = new JsonResult(ReasonPhrases.GetReasonPhrase(code));
        }

        /// <summary>
        /// 检测是否允许访问
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        private bool CheckAllowType(ControllerActionDescriptor controller)
        {
            var isAllow = controller.ControllerTypeInfo.CustomAttributes.Any(x => IsAllowType(x.AttributeType));
            if (isAllow) return isAllow;

            isAllow = controller.MethodInfo.CustomAttributes.Any(x => IsAllowType(x.AttributeType));
            return isAllow;
        }

        private bool IsAllowType(Type type) => _allowType.IsAssignableFrom(type);
    }

    public interface ISourcePointAuth
    {
        bool IsAuth(HttpContext context, ControllerActionDescriptor controllerActionDescriptor);
    }
}
