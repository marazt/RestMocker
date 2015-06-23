using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using FluentAssertions;
using Newtonsoft.Json;
using RestMocker.Core.Models;
using RestMocker.Core.Services;
using RestMocker.Model;
using Xunit;

namespace RestMocker.Core.Spec.Services
{

    public class ConfigurationServiceSpec
    {
        private ConfigurationService testee;

        public ConfigurationServiceSpec()
        {
            this.testee = new ConfigurationService(new NullLogger(), new HttpConfiguration());
        }

        [Fact]
        public void ShouldLoadFullConfigurationWithSuccess()
        {
            //Arrnage
            const string path = "config1.json";

            //Act
            testee.LoadConfiguration(path);

            //Assert
            var configuration = this.testee.GetConfiguration();
            configuration.Count.Should().Be(3);
            this.TestData(configuration[0], "test1", "test1/data", HttpMethodEnum.Get, 0, 23,
                new Dictionary<string, string> { { "response", "Hi 1!" } },
                new Dictionary<string, string> { { "header1", "value1" } }, HttpStatusCode.OK);

            this.TestData(configuration[1], "test2", "test2/bagr", HttpMethodEnum.Get, 5, 2,
                new Dictionary<string, string> { { "response", "Hi 2!" } },
                new Dictionary<string, string> { { "header2", "value2" } }, HttpStatusCode.OK);

            this.TestData(configuration[2], "test3", "test3/items", HttpMethodEnum.Post, 2, 0,
                new Dictionary<string, string> { { "response", "Hi 3!" } },
                new Dictionary<string, string> { { "header1", "value1" }, { "header3", "value3" } }, HttpStatusCode.NotFound);

            this.testee.HttpConfiguration.Routes.Count.Should().Be(3);

        }

        [Fact]
        public void ShouldNotLoadConfigurationIfFileDoesNotExist()
        {
            // Arrange
            const string path = "nonExistingConfig.config";

            // Act
            testee.GetConfiguration().Count.ShouldBeEquivalentTo(0);
            testee.LoadConfiguration(path);

            // Assert
            testee.GetConfiguration().Count.ShouldBeEquivalentTo(0);
            this.testee.HttpConfiguration.Routes.Count.Should().Be(0);
        }

        [Fact]
        public void ShouldLoadConfigurationWithoutMethodThrowsJsonSerializationException()
        {
            //Arrnage
            const string path = "config2.json";

            //Act
            Action fnc = () => testee.LoadConfiguration(path);

            //Assert
            fnc.ShouldThrow<JsonSerializationException>();
            this.testee.HttpConfiguration.Routes.Count.Should().Be(0);
        }

        [Fact]
        public void ShouldLoadConfigurationWithoutExpectedPropertiesThrowsJsonSerializationException()
        {
            //Arrnage
            const string path = "config3.json";

            //Act
            Action fnc = () => testee.LoadConfiguration(path);

            //Assert
            fnc.ShouldThrow<JsonSerializationException>();
            this.testee.HttpConfiguration.Routes.Count.Should().Be(0);
        }

        [Fact]
        public void ShouldLoadConfigurationWhisShouldNotRespond()
        {
            //Arrnage
            const string path = "config4.json";

            //Act
            testee.LoadConfiguration(path);

            //Assert
            this.testee.HttpConfiguration.Routes.Count.Should().Be(1);
        }

        [Fact]
        public void ShouldGetConfigurationByResourceWithNotNullResource()
        {
            //Arrnage
            const string resource = "test/resource";
            this.testee.SetConfiguration(new List<JsonConfigurationItem> { this.CreateTestInstance(HttpMethodEnum.Delete, resource) });

            //Act
            var expected = this.testee.GetConfigurationByResource("/" + resource, HttpMethodEnum.Delete);

            //Assert
            this.testee.GetConfiguration().Count.Should().Be(1);
            expected.Should().NotBeNull();
        }

        [Fact]
        public void ShouldNotGetConfigurationByResourceWithInvalidHttpMethod()
        {
            //Arrnage
            const string resource = "test/resource";
            this.testee.SetConfiguration(new List<JsonConfigurationItem> { this.CreateTestInstance(HttpMethodEnum.Delete, resource) });

            //Act
            var expected = this.testee.GetConfigurationByResource("/" + resource, HttpMethodEnum.Post);

            //Assert
            this.testee.GetConfiguration().Count.Should().Be(1);
            this.testee.HttpConfiguration.Routes.Count.Should().Be(1);
            expected.Should().BeNull();
        }

        public void ShouldGetConfigurationByResourceWithGenericResource()
        {
            //Arrnage
            const string resource = "test/{resource}";
            this.testee.SetConfiguration(new List<JsonConfigurationItem> { this.CreateTestInstance(HttpMethodEnum.Post, resource) });

            //Act
            var expected = this.testee.GetConfigurationByResource("/" + "test/bagr", HttpMethodEnum.Post);

            //Assert
            this.testee.GetConfiguration().Count.Should().Be(1);
            this.testee.HttpConfiguration.Routes.Count.Should().Be(1);
            expected.Should().NotBeNull();
        }

        [Fact]
        public void ShouldGetConfigurationByResourceWithNullResource()
        {
            //Arrnage
            const string resource = "test/resource";
            this.testee.SetConfiguration(new List<JsonConfigurationItem> { this.CreateTestInstance(HttpMethodEnum.Get, resource) });

            //Act
            var expected = this.testee.GetConfigurationByResource("/dummy", HttpMethodEnum.Get);

            //Assert
            this.testee.GetConfiguration().Count.Should().Be(1);
            this.testee.HttpConfiguration.Routes.Count.Should().Be(1);
            expected.Should().BeNull();
        }

        public void ShouldGetConfigurationByResourceByConcreteResourceWhenGenericResourceIsDefinedToo()
        {
            //Arrnage
            this.testee.SetConfiguration(new List<JsonConfigurationItem>{new JsonConfigurationItem
            {
                Method = HttpMethodEnum.Delete,
                MinDelay = 645,
                RandomDelay = 234,
                Name = "Generic",
                Resource = "test/{id}",
                Response = new ResponseItem()
            },
            new JsonConfigurationItem
            {
                Method = HttpMethodEnum.Put,
                MinDelay = 645,
                Name = "Concrete",
                Resource = "test/78",
                Response = new ResponseItem()
            }});


            //Act
            var expected = this.testee.GetConfigurationByResource("test/78", HttpMethodEnum.Put);

            //Assert
            this.testee.GetConfiguration().Count.Should().Be(2);
            this.testee.HttpConfiguration.Routes.Count.Should().Be(2);
            expected.Name.Should().Be("Concrete");
            expected.Should().NotBeNull();
        }

        [Fact]
        public void ShouldSetAndGetConfiguration()
        {
            //Arrnage
            //Act
            this.testee.GetConfiguration().Count.Should().Be(0);
            this.testee.HttpConfiguration.Routes.Count.Should().Be(0);
            this.testee.SetConfiguration(new List<JsonConfigurationItem>{new JsonConfigurationItem
            {
                Method = HttpMethodEnum.Delete,
                MinDelay = 645,
                RandomDelay = 234,
                Name = "Generic",
                Resource = "test/{id}",
                Response = new ResponseItem()
            },
            new JsonConfigurationItem
            {
                Method = HttpMethodEnum.Put,
                MinDelay = 645,
                RandomDelay = 234,
                Name = "Concrete",
                Resource = "test/78",
                Response = new ResponseItem()
            }});

            //Assert
            this.testee.GetConfiguration().Count.Should().Be(2);
            this.testee.HttpConfiguration.Routes.Count.Should().Be(2);
        }

        private void TestData(JsonConfigurationItem item, string name, string resource, string method, int minDelay, int randomDelay,
             Dictionary<string, string> json, Dictionary<string, string> headers, HttpStatusCode statusCode)
        {
            item.Name.Should().Be(name);
            item.Resource.Should().Be(resource);
            item.Method.Should().Be(method);
            item.MinDelay.Should().Be(minDelay);
            item.RandomDelay.Should().Be(randomDelay);
            item.Response.Should().NotBeNull();
            item.Response.Json.ShouldBeEquivalentTo(json);
            item.Response.Headers.ShouldBeEquivalentTo(headers);
            item.Response.StatusCode.ShouldBeEquivalentTo(statusCode);
        }

        private JsonConfigurationItem CreateTestInstance(string method, string resource)
        {
            return new JsonConfigurationItem
            {
                Method = method,
                MinDelay = 645,
                RandomDelay = 234,
                Name = "SomeName",
                Resource = resource,
                Response = new ResponseItem()
            };
        }

    }
}
