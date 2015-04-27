using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;

namespace RestMocker.Core.Controllers
{
    /// <summary>
    /// Common controller for handling all requests
    /// </summary>
    public class CommonController : ApiController
    {
        #region Methods

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        public async Task<IHttpActionResult> Get()
        {
            return await this.GetResponse();
        }

        /// <summary>
        /// Posts this instance.
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        public async Task<IHttpActionResult> Post()
        {
            return await this.GetResponse();
        }

        /// <summary>
        /// Puts this instance.
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        public async Task<IHttpActionResult> Put()
        {
            return await this.GetResponse();
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        public async Task<IHttpActionResult> Delete()
        {
            return await this.GetResponse();
        }

        /// <summary>
        /// Helper method to create response
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        private async Task<ResponseMessageResult> GetResponse()
        {
            var resourceUri = this.Request.RequestUri.AbsolutePath.ToLower();
            var method = this.Request.Method.Method;
            var conf = SimpleIocFactory.Instance.Configuration.GetConfigurationByResource(resourceUri, method);

            if (conf == null)
            {
                return new ResponseMessageResult(new HttpResponseMessage
                {
                    Content = new StringContent(string.Format("No response found for uri '{0}' and method '{1}', check your configuration", resourceUri, method))
                });
            }

            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(conf.Response.Json)),
                StatusCode = conf.Response.StatusCode
            };

            foreach (var header in conf.Response.Headers)
            {
                if ("content-type".Equals(header.Key.ToLower().Trim()))
                {
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(header.Value);
                }
                else
                {
                    response.Headers.Add(header.Key, new[] { header.Value });
                }
            }

            var delay = conf.MinDelay;
            if (delay > 0)
            {
                await Task.Delay(delay);
            }

            return new ResponseMessageResult(response);
        }

        #endregion Methods
    }
}
