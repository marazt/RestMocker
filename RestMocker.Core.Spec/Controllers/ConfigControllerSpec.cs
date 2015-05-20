using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using Newtonsoft.Json;
using RestMocker.Core.Controllers;
using RestMocker.Core.Models;
using RestMocker.Core.Services;
using RestMocker.Model;
using Xunit;

namespace RestMocker.Core.Spec.Controllers
{
    public class ConfigControllerSpec
    {
        private readonly ConfigController testee;


        public ConfigControllerSpec()
        {
            this.testee = new ConfigController();
        }

        [Fact]
        public void ShouldTestGetMethod()
        {
            // Arrange
            const int expectedLength = 1448;
            this.SetConfigurations();

            // Act
            var response = testee.GetConfiguration().Result as ResponseMessageResult;
            var data = response.Response.Content.ReadAsStringAsync().Result;

            // Assert
            var conf = JsonConvert.DeserializeObject<List<JsonConfigurationItem>>(data,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Error,
                        DefaultValueHandling = DefaultValueHandling.Include,

                    });

            conf.Count.ShouldBeEquivalentTo(4);
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
                Name = "test1",
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
                Name = "test2",
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
                Name = "test3",
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
                Name = "test4",
                Resource = "bagr",
                Response = new ResponseItem
                {
                    Headers = new Dictionary<string, string> { { "header4", "value4" } },
                    Json = new Dictionary<string, string> { { "value5", "val5" }, { "value6", "val6" } },
                    StatusCode = HttpStatusCode.Accepted
                }
            }});

            IocFactory.Instance.Register<IConfigurationService, ConfigurationService>(confMock);
        }

        [Fact]
        public void ShouldTestPostMethod()
        {
            // Arrange
            var configuration = new List<JsonConfigurationItem> { new JsonConfigurationItem
            {
                Method = HttpMethodEnum.Delete,
                Name = "test5",
                Resource = "bagr5",
                Response = new ResponseItem
                {
                    Headers = new Dictionary<string, string> { { "header5", "value5" } },
                    Json = new Dictionary<string, string> { { "value5", "val5" }, { "value6", "val6" } },
                    StatusCode = HttpStatusCode.Accepted
                }
            }};

            this.SetConfigurations();

            // Act
            var res = this.testee.PostConfiguration(configuration).Result;

            // Assert
            IocFactory.Instance.Configuration.GetConfiguration().Count.ShouldBeEquivalentTo(configuration.Count);
            IocFactory.Instance.Configuration.GetConfiguration()[0].ShouldBeEquivalentTo(configuration[0]);


        }
    }
}
