
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace PosNet.UseCases.Interfaces
{
    public interface IHandleBusinessError
    {
        void AddError(string message, int code = 500, string? fieldName = null);

        void AddValidationErrors(ValidationResult validationResult);

        bool HasErrors();

        int GetStatusCode();

        ProblemDetails CreateProblemDetails();
    }
}
