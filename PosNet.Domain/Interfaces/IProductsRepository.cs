using PosNet.Domain.Entities;

namespace PosNet.Domain.Interfaces
{
    public interface IProductsRepository : IRepository<Product>
    {
        Task<Product?> GetProductWithDetailsById(Guid id);

        Task<IEnumerable<Product>> GetProductsByCategory(Guid categoryId);

        Task<IEnumerable<Product>> GetLowStockProducts(int threshold);
    }
}
