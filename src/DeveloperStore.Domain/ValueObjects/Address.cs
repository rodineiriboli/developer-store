using DeveloperStore.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Domain.ValueObjects
{
    [Owned]
    public class Address : ValueObject
    {
        public string City { get; private set; }
        public string Street { get; private set; }
        public int Number { get; private set; }
        public string ZipCode { get; private set; }
        public GeoLocation GeoLocation { get; private set; }

        // Construtor público
        public Address(string city, string street, int number, string zipCode, GeoLocation geoLocation)
        {
            City = city;
            Street = street;
            Number = number;
            ZipCode = zipCode;
            GeoLocation = geoLocation;
        }

        // Construtor privado para o EF
        private Address() { }

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