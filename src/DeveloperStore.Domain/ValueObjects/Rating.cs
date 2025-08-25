using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Exceptions;

namespace DeveloperStore.Domain.ValueObjects
{
    public class Rating : ValueObject
    {
        public decimal Rate { get; }
        public int Count { get; }

        public Rating(decimal rate, int count)
        {
            if (rate < 0 || rate > 5)
                throw new DomainException("Rate must be between 0 and 5");

            if (count < 0)
                throw new DomainException("Count must be greater than or equal to zero");

            Rate = rate;
            Count = count;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Rate;
            yield return Count;
        }
    }
}