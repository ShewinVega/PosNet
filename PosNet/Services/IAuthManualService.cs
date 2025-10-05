using PosNet.DTOs;
using PosNet.Models;

namespace PosNet.Services
{
    public interface IAuthManualService
    {
        public Task<User?> Register(AuthDto request);
        public Task<TokenResponseDto?> Login(AuthDto request, User user);
        public Task<User?> GetUser(string username);

        public Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
        public Task<string> GenerateAndSaveRefreshTokenAsync(User user);

    }
}
