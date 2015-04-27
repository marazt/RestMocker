using FluentAssertions;
using Xunit;

namespace RestMocker.Core.Spec
{
    public class SimpleIocFactorySpec
    {
        [Fact]
        public void ShouldTestSimpleIoCCreation()
        {
            // Arrange
            // Act
            var testee = SimpleIocFactory.Instance;

            // Assert
            testee.Should().NotBeNull();
            testee.Configuration.Should().NotBeNull();
            testee.Logger.Should().NotBeNull();
        }

        [Fact]
        public void ShouldTestSingletonInstance()
        {
            // Arrange
            // Act
            var testee1 = SimpleIocFactory.Instance;
            var testee2 = SimpleIocFactory.Instance;

            // Assert
            testee1.GetHashCode().ShouldBeEquivalentTo(testee2.GetHashCode());
        }
    }
}
