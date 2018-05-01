using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
namespace SourcePoint.Infrastructure.Extensions.MVCExtension.ControllerExtensions
{
    /// <summary>
    /// 控制器扩展
    /// </summary>
    public static class ControllerExtension
    {
        /// <summary>
        /// 把ModelState错误 添加到 BadRequestObjectResult对象里
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="key">错误Key</param>
        /// <param name="error">错误信息</param>
        /// <returns>BadRequestObjectResult</returns>
        public static BadRequestObjectResult ModelStateError(this ControllerBase controller, string key, string error)
        {
            controller.AddStateError(key, error);
            return controller.BadRequest(controller.ModelState);
        }

        /// <summary>
        /// 向 ModelState里添加错误信息
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="key">键</param>
        /// <param name="error">错误信息</param>
        /// <returns>ModelState</returns>
        public static ModelStateDictionary AddStateError(this ControllerBase controller, string key, string error)
        {
            controller.ModelState.AddModelError(key, error);
            return controller.ModelState;
        }

        /// <summary>
        /// 获取注入服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <returns>T</returns>
        public static T Services<T>(this ControllerBase controller)
        {
            return controller.HttpContext.RequestServices.GetService<T>();
        }
    }
}
