

namespace PosNet.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? RefreshToken {  get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public User(string username, string email, string passwordHash, int roleId)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            RoleId = roleId;
        }

        public User() { }
    }
}
