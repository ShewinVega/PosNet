
using Microsoft.AspNetCore.Mvc;

namespace PosNet.UseCases.Interfaces
{
    public interface IHandleBusinessError
    {
        void AddError(string message, int code = 500, string? fieldName = null);

        bool HasErrors();

        int GetStatusCode();

        ProblemDetails CreateProblemDetails();
    }
}
