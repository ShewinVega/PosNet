

using Microsoft.AspNetCore.Http;

namespace PosNet.Infrastructure.ProblemsDetail
{
    public class ProblemTypes
    {
        public const string ValidationError = "https://problems-registry.smartbear.com/validation-error/";
        public const string BadRequest = "https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request";
        public const string Forbidden = "https://datatracker.ietf.org/doc/html/rfc9110#name-403-forbidden";
        public const string NotFound = "https://datatracker.ietf.org/doc/html/rfc9110#name-404-not-found";
        public const string Unauthorized = "https://datatracker.ietf.org/doc/html/rfc9110#name-401-unauthorized";
        public const string InternalServer = "https://datatracker.ietf.org/doc/html/rfc9110#name-500-internal-server-error";


        public ErrorDetail SetProblemType(int code)
        {
            switch (code)
            {
                case 400:
                    return new ErrorDetail
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Type = BadRequest
                    };
                case 401:
                    return new ErrorDetail
                    {
                        Code = StatusCodes.Status401Unauthorized,
                        Type = Unauthorized
                    };
                case 404:
                    return new ErrorDetail
                    {
                        Code = StatusCodes.Status404NotFound,
                        Type = NotFound
                    };
                case 403:
                    return new ErrorDetail
                    {
                        Code = StatusCodes.Status403Forbidden,
                        Type = Forbidden
                    };
                default:
                    return new ErrorDetail
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Type = InternalServer
                    };
            }
        }
    }
}
