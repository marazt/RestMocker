using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using RestMocker.Core.Models;

namespace RestMocker.Core.Controllers
{
    /// <summary>
    /// Common controller for handling all requests
    /// </summary>
    public class CommonController : ApiController
    {
        #region Methods

        /// <summary>
        /// Method which handles all incomming request and resolves which
        /// response will be returned to the client.
        /// It accepts 'all' common HTTP methods
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        /// 
       [AcceptVerbs(HttpMethodEnum.Delete, HttpMethodEnum.Get, HttpMethodEnum.Head, HttpMethodEnum.Options, HttpMethodEnum.Patch, HttpMethodEnum.Post, HttpMethodEnum.Put)]
        public async Task<IHttpActionResult> Execute()
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
