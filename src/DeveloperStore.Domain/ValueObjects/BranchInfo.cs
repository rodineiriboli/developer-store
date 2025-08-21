using DeveloperStore.Domain.Common;

namespace DeveloperStore.Domain.ValueObjects
{
    public class BranchInfo : ValueObject
    {
        public int BranchId { get; }
        public string Name { get; }
        public string Location { get; }

        public BranchInfo(int branchId, string name, string location)
        {
            BranchId = branchId;
            Name = name;
            Location = location;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return BranchId;
            yield return Name;
            yield return Location;
        }
    }
}
