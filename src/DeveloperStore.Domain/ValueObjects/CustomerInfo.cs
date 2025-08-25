using DeveloperStore.Domain.Common;

namespace DeveloperStore.Domain.ValueObjects
{
    public class CustomerInfo : ValueObject
    {
        public Guid CustomerId { get; }
        public string Name { get; }
        public string Email { get; }

        public CustomerInfo(Guid customerId, string name, string email)
        {
            CustomerId = customerId;
            Name = name;
            Email = email;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CustomerId;
            yield return Name;
            yield return Email;
        }
    }
}
