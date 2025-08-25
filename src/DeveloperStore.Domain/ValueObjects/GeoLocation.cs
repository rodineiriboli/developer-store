using DeveloperStore.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Domain.ValueObjects
{
    [Owned]
    public class GeoLocation : ValueObject
    {
        public string Lat { get; private set; }
        public string Long { get; private set; }

        // Construtor público
        public GeoLocation(string lat, string @long)
        {
            Lat = lat;
            Long = @long;
        }

        // Construtor privado para o EF
        private GeoLocation() { }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Lat;
            yield return Long;
        }
    }
}