using FluentAssertions;
using Xunit;

namespace RestMocker.Console.Spec
{
    public class AppConfigSpec
    {

        [Fact]
        public void ShouldGetHost()
        {
            //Arrnage
            //Act
            //Assert
            AppConfig.Port.Should().Be(8654);
            AppConfig.Host.Should().Be("http://localhost");
        }
    }
}
