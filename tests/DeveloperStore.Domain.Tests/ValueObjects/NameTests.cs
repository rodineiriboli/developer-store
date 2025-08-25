using DeveloperStore.Domain.ValueObjects;
using System.Xml.Linq;
using Xunit;

namespace DeveloperStore.Domain.Tests.ValueObjects
{
    [Trait("ValueObjects", "Name")]
    public class NameTests
    {
        [Fact]
        public void Constructor_ShouldCreateName_WithValidParameters()
        {
            // Arrange & Act
            var name = new Name("John", "Doe");

            // Assert
            Assert.Equal("John", name.FirstName);
            Assert.Equal("Doe", name.LastName);
            Assert.Equal("John Doe", name.FullName);
        }

        [Theory]
        [InlineData("John", "Doe", "John", "Doe", true)]
        [InlineData("John", "Doe", "Jane", "Doe", false)]
        [InlineData("John", "Doe", "John", "Smith", false)]
        public void Equals_ShouldReturnCorrectResult(string first1, string last1, string first2, string last2, bool expected)
        {
            // Arrange
            var name1 = new Name(first1, last1);
            var name2 = new Name(first2, last2);

            // Act & Assert
            Assert.Equal(expected, name1.Equals(name2));
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameValue_ForEqualObjects()
        {
            // Arrange
            var name1 = new Name("John", "Doe");
            var name2 = new Name("John", "Doe");

            // Act & Assert
            Assert.Equal(name1.GetHashCode(), name2.GetHashCode());
        }
    }
}