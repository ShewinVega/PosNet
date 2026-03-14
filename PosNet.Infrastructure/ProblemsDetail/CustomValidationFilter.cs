using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace PosNet.Infrastructure.ProblemsDetail
{
    public class CustomValidationFilter(ProblemDetailsFactory problemDetailsFactory) : IActionFilter
    {
        private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {

                var problemDetails = _problemDetailsFactory.CreateValidationProblemDetails(
                    context.HttpContext,
                    context.ModelState,
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Validation Error",
                    type: ProblemTypes.ValidationError,
                    detail: "One or more validation errors occurred."
                );

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
