namespace PosNet.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        
        public required string Name { get; set; }
        public string? Description { get; set; }

        public required decimal Stock { get; set; }

        public required decimal UnitPrice { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        public Product(string name, decimal stock, decimal unitPrice, int categoryId, int brandId, string? description = null)
        {
            Name = name;
            Description = description;
            Stock = stock;
            UnitPrice = unitPrice;
            CategoryId = categoryId;
            BrandId = brandId;
        }

        public Product() { }

    }
}
