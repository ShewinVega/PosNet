namespace PosNet.Domain.Entities
{
    public class Brand
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; } = [];

        public Brand(string name)
        {
            Name = name;
        }

        public Brand() { }
    }
}
