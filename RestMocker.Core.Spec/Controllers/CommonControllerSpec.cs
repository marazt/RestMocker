using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using RestMocker.Core.Controllers;
using RestMocker.Core.Models;
using RestMocker.Core.Services;
using RestMocker.Model;
using Xunit;

namespace RestMocker.Core.Spec.Controllers
{
    public class CommonControllerSpec
    {

        [Fact]
        public void ShouldTestGetMethod()
        {
            // Arrange
            const string url = "http://www.contoso.com/bagr/test/8";
            var controller = this.GetTestController(url, HttpMethodEnum.Get.ToUpper());
            const string expectedData = "{\"value1\":\"val2\"}";
            this.SetConfigurations();

            // Act
            var response = ((controller.Execute()).Result as ResponseMessageResult).Response;

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            response.Headers.Count().ShouldBeEquivalentTo(0);
            var data = response.Content.ReadAsStringAsync().Result;
            data.ShouldBeEquivalentTo(expectedData);
        }

        [Fact]
        public void ShouldTestPostMethod()
        {
            // Arrange
            const string url = "http://www.contoso.com/bagr/test/54541";
            var controller = this.GetTestController(url, HttpMethodEnum.Post);
            const string expectedData = "{\"value2\":\"val2\"}";
            this.SetConfigurations();

            // Act
            var response = ((controller.Execute()).Result as ResponseMessageResult).Response;

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.InternalServerError);
            response.Headers.Count().ShouldBeEquivalentTo(0);
            var data = response.Content.ReadAsStringAsync().Result;
            data.ShouldBeEquivalentTo(expectedData);
        }

        [Fact]
        public void ShouldTestPatchMethod()
        {
            // Arrange
            const string url = "http://www.contoso.com/bagr/test/11";
            var controller = this.GetTestController(url, HttpMethodEnum.Patch);
            const string expectedData = "{\"value2\":\"val6\"}";
            this.SetConfigurations();

            // Act
            var response = ((controller.Execute()).Result as ResponseMessageResult).Response;

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.InternalServerError);
            response.Headers.Count().ShouldBeEquivalentTo(0);
            var data = response.Content.ReadAsStringAsync().Result;
            data.ShouldBeEquivalentTo(expectedData);
        }

        [Fact]
        public void ShouldTestPutMethod()
        {
            // Arrange
            const string url = "http://www.contoso.com/bagr/test/ssssssssss/item";
            var controller = this.GetTestController(url, HttpMethodEnum.Put);
            const string expectedData = "{\"value3\":\"val3\",\"value4\":\"val4\"}";
            this.SetConfigurations();

            // Act
            var response = ((controller.Execute()).Result as ResponseMessageResult).Response;

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);
            response.Headers.Count().ShouldBeEquivalentTo(0);
            var data = response.Content.ReadAsStringAsync().Result;
            data.ShouldBeEquivalentTo(expectedData);
        }

        [Fact]
        public void ShouldTestDeleteMethod()
        {
            // Arrange
            const string url = "http://www.contoso.com/bagr";
            var controller = this.GetTestController(url, HttpMethodEnum.Delete);
            const string expectedData = "{\"value5\":\"val5\",\"value6\":\"val6\"}";
            this.SetConfigurations();

            // Act
            var response = ((controller.Execute()).Result as ResponseMessageResult).Response;

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Accepted);
            response.Headers.Count().ShouldBeEquivalentTo(1);
            response.Headers.GetValues("header4").ElementAt(0).ShouldBeEquivalentTo("value4");
            var data = response.Content.ReadAsStringAsync().Result;
            data.ShouldBeEquivalentTo(expectedData);
        }

        [Fact]
        public void ShouldTestHeadMethod()
        {
            // Arrange
            const string url = "http://www.contoso.com/bagr";
            var controller = this.GetTestController(url, HttpMethodEnum.Head);
            const string expectedData = "{}";
            this.SetConfigurations();

            // Act
            var response = ((controller.Execute()).Result as ResponseMessageResult).Response;

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
            response.Headers.Count().ShouldBeEquivalentTo(1);
            response.Headers.GetValues("Encoding").ElementAt(0).ShouldBeEquivalentTo("utf-8");
            var data = response.Content.ReadAsStringAsync().Result;
            data.ShouldBeEquivalentTo(expectedData);
        }

        [Fact]
        public void ShouldTestOptionsMethod()
        {
            // Arrange
            const string url = "http://www.contoso.com/bagr";
            var controller = this.GetTestController(url, HttpMethodEnum.Options);
            const string expectedData = "{\"key\":\"654\"}";
            this.SetConfigurations();

            // Act
            var response = ((controller.Execute()).Result as ResponseMessageResult).Response;

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            response.Headers.Count().ShouldBeEquivalentTo(0);
            var data = response.Content.ReadAsStringAsync().Result;
            data.ShouldBeEquivalentTo(expectedData);
        }

        /// <summary>
        /// Helper method to mock configuration data
        /// </summary>
        private void SetConfigurations()
        {
            var confMock = new ConfigurationService(new NullLogger(), new HttpConfiguration());
            confMock.SetConfiguration(new List<JsonConfigurationItem>{new JsonConfigurationItem
            {
                Method = HttpMethodEnum.Get,
                Name = "GetTest",
                Resource = "bagr/test/8",
                Response = new ResponseItem
                {
                    Headers = new Dictionary<string, string> { { "Content-type", "application/json" } },
                    Json = new Dictionary<string, string> { { "value1", "val2" } },
                    StatusCode = HttpStatusCode.OK
                }
            },
            new JsonConfigurationItem
            {
                Method = HttpMethodEnum.Post,
                Name = "PostTest",
                Resource = "bagr/test/{id}",
                Response = new ResponseItem
                {
                    Headers = new Dictionary<string, string> { { "Content-type", "application/json" } },
                    Json = new Dictionary<string, string> { { "value2", "val2" } },
                    StatusCode = HttpStatusCode.InternalServerError
                }
            },
            new JsonConfigurationItem
            {
                Method = HttpMethodEnum.Put,
                Name = "PutTest",
                Resource = "bagr/test/{hash}/item",
                Response = new ResponseItem
                {
                    Headers = new Dictionary<string, string> { { "Content-type", "application/json" } },
                    Json = new Dictionary<string, string> { { "value3", "val3" }, { "value4", "val4" } },
                    StatusCode = HttpStatusCode.BadRequest
                }
            },
            new JsonConfigurationItem
            {
                Method = HttpMethodEnum.Delete,
                Name = "DeleteTest",
                Resource = "bagr",
                Response = new ResponseItem
                {
                    Headers = new Dictionary<string, string> { { "header4", "value4" } },
                    Json = new Dictionary<string, string> { { "value5", "val5" }, { "value6", "val6" } },
                    StatusCode = HttpStatusCode.Accepted
                }
            },
            new JsonConfigurationItem
            {
                Method = HttpMethodEnum.Patch,
                Name = "PatchTest",
                Resource = "bagr/test/{id}",
                Response = new ResponseItem
                {
                    Headers = new Dictionary<string, string> { { "Content-type", "application/json" } },
                    Json = new Dictionary<string, string> { { "value2", "val6" } },
                    StatusCode = HttpStatusCode.InternalServerError
                }
            },
            new JsonConfigurationItem
            {
                Method = HttpMethodEnum.Head,
                Name = "HeadTest",
                Resource = "bagr",
                Response = new ResponseItem
                {
                    Headers = new Dictionary<string, string> { { "Encoding", "utf-8" } },
                    Json = new Dictionary<string, string> (),
                    StatusCode = HttpStatusCode.NotFound
                }
            },
            new JsonConfigurationItem
            {
                Method = HttpMethodEnum.Options,
                Name = "OptionsTest",
                Resource = "bagr",
                Response = new ResponseItem
                {
                    Headers = new Dictionary<string, string> (),
                    Json = new Dictionary<string, string> { { "key", "654" } },
                    StatusCode = HttpStatusCode.OK
                }
            }
            });

            IocFactory.Instance.Register<IConfigurationService, ConfigurationService>(confMock);
        }

        /// <summary>
        /// Helper method to get test controller
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="method">Http method</param>
        /// <returns>CommonController instance</returns>
        private CommonController GetTestController(string url, string method)
        {
            var controller = new CommonController
            {
                Request = new HttpRequestMessage { RequestUri = new Uri(url), Method = new HttpMethod(method) },
                Configuration = new HttpConfiguration()

            };
            return controller;
        }
    }
}
