
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PosNet.UseCases.Interfaces;

namespace PosNet.Infrastructure.ProblemsDetail
{
    public class HandleBusinessError(
        ProblemTypes problemType, 
        IHttpContextAccessor httpContextAccessor
        ) : IHandleBusinessError
    {
        private readonly ProblemTypes _problemType = problemType;
        private readonly HttpContext? _context = httpContextAccessor.HttpContext;

        private Dictionary<string, List<String>> GroupedErrors { get; set; } = [];
        private readonly List<int> status = [];

        public void AddError(string message, int code = StatusCodes.Status500InternalServerError, string? fieldName = null)
        {
            // Check if we have a field.
            string key = fieldName ?? "General";

            if (!GroupedErrors.TryGetValue(key, out var _))
            {
                GroupedErrors[key] = [];
            }

            // Check status code.
            status.Add(code);
            GroupedErrors[key].Add(message);
        }

        public void AddValidationErrors(ValidationResult validationResult)
        {
            Console.WriteLine(validationResult.Errors);
            foreach (var error in validationResult.Errors)
            {
                AddError(error.ErrorMessage, StatusCodes.Status400BadRequest, error.PropertyName);
            }
        }

        public bool HasErrors()
        {
            if(GroupedErrors.Any()) return true;
            return false;
        }

        public int GetStatusCode()
        {
            if (!status.Any()) return StatusCodes.Status500InternalServerError;
            
            if(status.Contains(StatusCodes.Status500InternalServerError)) return StatusCodes.Status500InternalServerError;

            return status.First();
        }

        public ProblemDetails CreateProblemDetails()
        {
            // set error type
            var errorType = _problemType.SetProblemType(status.First());

            var problemDetails = new ProblemDetails
            {
                Type = errorType.Type,
                Status = errorType.Code,
                Title = "Business validation error",
                Detail = "One or more business errors occurred.",
                Instance = _context?.Request.Path
            };
            // Add the grouped errors to the Extensions property of ProblemDetails.
            problemDetails.Extensions["errors"] = GroupedErrors;
            problemDetails.Extensions["traceId"] = _context?.TraceIdentifier ?? "Unknown";

            return problemDetails;
        }
    }
}
