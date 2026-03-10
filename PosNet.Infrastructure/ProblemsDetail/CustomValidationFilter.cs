using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PosNet.Infrastructure.ProblemsDetail
{
    public class CustomValidationFilter : IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var groupedErrors = new Dictionary<string, List<string>>();
            if (!context.ModelState.IsValid)
            {

                foreach (var key in context.ModelState.Keys)
                {
                    var errors = context.ModelState[key].Errors.Select(e => e.ErrorMessage).ToList();
                    groupedErrors.Add(key, errors);
                }

                var problemDetails = new ProblemDetails
                {
                    Type = ProblemTypes.ValidationError,
                    Title = "Validation Error",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "One or more validation errors occurred.",
                };

                problemDetails.Extensions.Add("errors", groupedErrors);


                context.Result = new ObjectResult(problemDetails)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
