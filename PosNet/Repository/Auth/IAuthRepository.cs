using PosNet.DTOs;
using PosNet.Models;

namespace PosNet.Repository.Auth
{
    public interface IAuthRepository
    {

        public Task Register(User user);
        public Task<bool> UsernameExists(string username);

        public Task<User?> GetUser(string username);

        public Task<User?> GetUserById(Guid userId);

        public Task Save();
    }
}
