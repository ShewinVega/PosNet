
using PosNet.UseCases.Dtos.Auth;

namespace PosNet.UseCases.Interfaces
{
    public interface IAuthService
    {

        Task<Result<UserDto>> Register(RegisterDto request);

        Task<Result<TokenDto>> Login(LoginDto request);

        Task<Result<TokenDto>> RefreshAccessTokenAsync(RefreshTokenDto request);
    }
}
