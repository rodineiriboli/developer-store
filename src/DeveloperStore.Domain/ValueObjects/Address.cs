using DeveloperStore.Domain.Common;

namespace DeveloperStore.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        public string City { get; }
        public string Street { get; }
        public int Number { get; }
        public string ZipCode { get; }
        public GeoLocation GeoLocation { get; }

        public Address(string city, string street, int number, string zipCode, GeoLocation geoLocation)
        {
            City = city;
            Street = street;
            Number = number;
            ZipCode = zipCode;
            GeoLocation = geoLocation;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return City;
            yield return Street;
            yield return Number;
            yield return ZipCode;
            yield return GeoLocation;
        }
    }
}