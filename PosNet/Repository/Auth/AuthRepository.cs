using Microsoft.EntityFrameworkCore;
using PosNet.DTOs;
using PosNet.Models;

namespace PosNet.Repository.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Register(User request)
        {
            try
            {
                await _context.Users.AddAsync(request);
            } catch(Exception error)
            {
                Console.WriteLine($"There was an error in Auth repository: {error}");
                throw new Exception(error.Message);
            }
        }

        public async Task<bool> UsernameExists(string username)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Username == username))
                {
                    return true;
                }

                return false;
            }
            catch (Exception error)
            {
                Console.WriteLine($"There was an error in Auth repository: {error}");
                throw new Exception(error.Message);
            }
        }

        public async Task<User?> GetUser(string username)
        {
           try
           {
                return await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
           } catch(Exception error)
            {
                Console.WriteLine($"There was an error in Auth repository: {error}");
                throw new Exception(error.Message);
            }
        }


        public async Task Save()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception error)
            {
                Console.WriteLine($"There was an error in Auth repository: {error}");
                throw new Exception(error.Message);
            }

        }

        public async Task<User?> GetUserById(Guid userId)
        {
            try
            {
                return await _context.Users.FindAsync(userId);

            }
            catch (Exception error)
            {
                Console.WriteLine($"There was an error in Auth repository: {error}");
                throw new Exception(error.Message);
            }
        }
    }
}
