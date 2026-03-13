
using PosNet.Domain.Constants;
using PosNet.Domain.Interfaces;
using PosNet.Infrastructure.Persistence;

namespace PosNet.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext appDbContext) : IUserRepository
    {
        private readonly AppDbContext _context = appDbContext;


        public IQueryable<User> GetAllUsersAsIQueryable()
        {
            return _context.Users.Include(r => r.Role)
                .Where(r => r.Role.Name != Roles.Admin)
                .AsNoTracking();
        }

        public async Task Register(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetUserByName(string name)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == name.ToLower());
        }

        public async Task<User?> GetUserByNameOrEmail(string identifier)
        {
            return await _context.Users
                .Include(r => r.Role)
                .ThenInclude(rp => rp.RolesPermissions)
                .ThenInclude(p => p.Permission)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == identifier.ToLower() 
                || u.Email.ToLower() == identifier.ToLower());
        }

        public async Task<User?> GetUserByRefreshToken(string refreshToken)
        {
            return await _context.Users
                .Include(r => r.Role)
                .ThenInclude(rp => rp.RolesPermissions)
                .ThenInclude(p => p.Permission)
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }
    }
}
