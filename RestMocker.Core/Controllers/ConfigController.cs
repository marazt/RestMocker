using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using RestMocker.Core.Models;
using RestMocker.Model;

namespace RestMocker.Core.Controllers
{
    /// <summary>
    /// Info controller for handling all requests
    /// </summary>
    public class ConfigController : ApiController
    {
        #region Constants

        private const string ContentTypeJson = "application/json";

        #endregion Constants

        #region Methods

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        [AcceptVerbs(HttpMethodEnum.Get)]
        public async Task<IHttpActionResult> GetConfiguration()
        {
            var conf = JsonConvert.SerializeObject(SimpleIocFactory.Instance.Configuration.GetConfiguration(), Formatting.Indented);
            var response = new HttpResponseMessage
            {
                Content = new StringContent(conf, Encoding.UTF8, ContentTypeJson),
                StatusCode = HttpStatusCode.OK
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentTypeJson);
            return new ResponseMessageResult(response);
        }

        /// <summary>
        /// Method for setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>HttpResponseMessage instance</returns>
        [AcceptVerbs(HttpMethodEnum.Post)]
        public async Task<IHttpActionResult> PostConfiguration([FromBody]IEnumerable<JsonConfigurationItem> configuration)
        {
            SimpleIocFactory.Instance.Configuration.SetConfiguration(configuration);
            return Ok();
        }

        #endregion Methods
    }
}
