using DeveloperStore.Domain.Common;

namespace DeveloperStore.Domain.ValueObjects
{
    public class Name : ValueObject
    {
        public string FirstName { get; }
        public string LastName { get; }

        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FullName => $"{FirstName} {LastName}";

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
}