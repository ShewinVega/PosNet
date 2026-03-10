using PosNet.UseCases.Dtos.Auth;

namespace PosNet.UseCases.Interfaces
{
    public interface ITokenAuthService
    {

        Task<Result<TokenDto>> CreateTokenResponse(User user);

    }
}
