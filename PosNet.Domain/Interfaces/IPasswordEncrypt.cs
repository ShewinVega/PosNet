

namespace PosNet.Domain.Interfaces
{
    public interface IPasswordEncrypt
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }
}
