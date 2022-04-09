using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace Payment.Attributes
{
    public class ValidateViewModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            context.Result = BuildValidationErrorResponse(context.ModelState);
        }

        private static BadRequestObjectResult BuildValidationErrorResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Select(x => new ErrorModel
            {
                Field = x.Key,
                Message = x.Value.Errors.FirstOrDefault()?.ErrorMessage
            }).ToList();

            return new BadRequestObjectResult(errors);
        }
    }
}
