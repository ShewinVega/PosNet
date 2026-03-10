using PosNet.Domain.Interfaces;
using BCrypt.Net;

namespace PosNet.Infrastructure.Security
{
    public class BCryptHasher : IPasswordEncrypt
    {
        public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
