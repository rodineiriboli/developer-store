using DeveloperStore.Domain.Common;

namespace DeveloperStore.Domain.ValueObjects
{
    public class ProductInfo : ValueObject
    {
        public int ProductId { get; }
        public string Name { get; }
        public string Description { get; }

        public ProductInfo(int productId, string name, string description)
        {
            ProductId = productId;
            Name = name;
            Description = description;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ProductId;
            yield return Name;
            yield return Description;
        }
    }
}
