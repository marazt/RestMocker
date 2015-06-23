using System.Net.Http;
using System.Web.Http.Controllers;
using FluentAssertions;
using Moq;
using RestMocker.Core.Utils;
using Xunit;

namespace RestMocker.Core.Spec.Utils
{
    public class ValidateModelAttributeSpec
    {
        [Fact]
        public void ShouldTestThatErrorResponseIsSetWhenModelStateIsNotValid()
        {
            // Arrange
            var contextMock = GetMockedActionContext();
            contextMock.ModelState.AddModelError("key", "error");

            var filter = new ValidateModelAttribute();

            // Act
            filter.OnActionExecuting(contextMock);

            // Assert
            contextMock.Response.Should().NotBeNull();
        }

        [Fact]
        public void ShouldTestThatErrorResponseIsNotSetWhenModelStateIsValid()
        {
            // Arrange
            var contextMock = GetMockedActionContext();
            var filter = new ValidateModelAttribute();

            // Act
            filter.OnActionExecuting(contextMock);

            // Assert
            contextMock.Response.Should().BeNull();
        }

        private static HttpActionContext GetMockedActionContext()
        {
            var controllerContext = new HttpControllerContext { Request = new HttpRequestMessage() };
            var descriptor = new Mock<HttpActionDescriptor>() { CallBase = true }.Object;
            var contextMock = new HttpActionContext(controllerContext, descriptor);
            return contextMock;
        }
    }
}
