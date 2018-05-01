using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SourcePoint.Infrastructure.Extensions.MVCExtension.Filters
{
    public class ModelValidatorFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }
            foreach (var argument in context.ActionArguments)
            {
                if (argument.Value != null) continue;
                var param = context.ActionDescriptor.Parameters.First(m => m.Name == argument.Key) as ControllerParameterDescriptor;
                if (param.ParameterInfo.CustomAttributes.Any(m => m.AttributeType == typeof(RequiredAttribute)))
                {
                    context.Result = new BadRequestResult();
                    return;
                }
            }
        }
    }
}
