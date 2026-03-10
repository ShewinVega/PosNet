namespace PosNet.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBrandRepository Brand { get; }
        ICategoryRepository Category { get; }
        IProductsRepository Product { get; }
        IUserRepository User { get; }
        IRoleRepository Role { get; }
        Task<int> Save();
       
    }
}
