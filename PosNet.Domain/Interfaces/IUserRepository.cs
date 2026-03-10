using PosNet.Domain.Entities;

namespace PosNet.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task Register(User user);

        IQueryable<User> GetAllUsersAsIQueryable();
        Task<User?> GetUserByRefreshToken(string refreshToken);

        Task<User?> GetUserByEmail(string email);

        Task<User?> GetUserByName(string name);

        Task<User?> GetUserById(int id);

        Task<User?> GetUserByNameOrEmail(string identifier);
    }
}
