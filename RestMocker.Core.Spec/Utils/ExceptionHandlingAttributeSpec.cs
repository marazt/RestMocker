using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using FluentAssertions;
using RestMocker.Core.Utils;
using Xunit;

namespace RestMocker.Core.Spec.Utils
{
    public class ExceptionHandlingAttributeSpec
    {
        [Fact]
        public void ShouldTestExceptionHandling()
        {
            // Arrange
            var request = new HttpRequestMessage();
            var actionContext = InitializeActionContext(request);
            var httpActionExectuedContext = new HttpActionExecutedContext(actionContext, new DivideByZeroException());
            var exceptionHandlingAttribute = new ExceptionHandlingAttribute();

            // Act
            Action act = () => exceptionHandlingAttribute.OnException(httpActionExectuedContext);
            act.ShouldThrow<HttpResponseException>();
        }

        // Code taken from http://chimera.labs.oreilly.com/books/1234000001708/ch17.html#_unit_testing_an_actionfilterattribute
        /// <summary>
        /// Initializes the action context.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>HttpActionContext instance</returns>
        private HttpActionContext InitializeActionContext(HttpRequestMessage request)
        {
            var configuration = new HttpConfiguration();

            var routeData = new HttpRouteData(new HttpRoute(),
                new HttpRouteValueDictionary
                {
                    {"controller", "Issues"}
                });

            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            var controllerContext = new HttpControllerContext(configuration, routeData, request);
            var actionContext = new HttpActionContext
            {
                ControllerContext = controllerContext
            };

            return actionContext;
        }
    }
}
