using DeveloperStore.Domain.Common;

namespace DeveloperStore.Domain.ValueObjects
{
    public class GeoLocation : ValueObject
    {
        public string Lat { get; }
        public string Long { get; }

        public GeoLocation(string lat, string @long)
        {
            Lat = lat;
            Long = @long;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Lat;
            yield return Long;
        }
    }
}