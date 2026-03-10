using PosNet.Domain.Interfaces;
using PosNet.Infrastructure.Persistence;
namespace PosNet.Infrastructure.Repositories
{
    public class UnitOfWork(AppDbContext appDbContext): IUnitOfWork
    {
        // Database context for the unit of work and all the repositories it manages
        private readonly AppDbContext _context = appDbContext;

        private IRoleRepository? _roleRepository;
        private IUserRepository? _userRepository;

        // Repositories for the unit of work, initialized lazily when accessed
        
        public IUserRepository User => _userRepository ??= new UserRepository(_context);

        public IBrandRepository Brand => throw new NotImplementedException();

        public ICategoryRepository Category => throw new NotImplementedException();

        public IProductsRepository Product => throw new NotImplementedException();

        public IRoleRepository Role => _roleRepository ??= new RoleRepository(_context);

        public async Task<int> Save() => await _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this); // Make sure the garbage collector doesn't call the finalizer after disposing
        }
    }
}
