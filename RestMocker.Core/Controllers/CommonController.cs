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
        /// Handles Delete method
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        [AcceptVerbs(HttpMethodEnum.Delete)]
        public async Task<IHttpActionResult> Delete()
        {
            return await this.Execute();
        }

        /// <summary>
        /// Handles Get method
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        [AcceptVerbs(HttpMethodEnum.Get)]
        public async Task<IHttpActionResult> Get()
        {
            return await this.Execute();
        }

        /// <summary>
        /// Handles Head method
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        [AcceptVerbs(HttpMethodEnum.Head)]
        public async Task<IHttpActionResult> Head()
        {
            return await this.Execute();
        }

        /// <summary>
        /// Handles Options method
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        [AcceptVerbs(HttpMethodEnum.Options)]
        public async Task<IHttpActionResult> Options()
        {
            return await this.Execute();
        }

        /// <summary>
        /// Handles PAtch method
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        [AcceptVerbs(HttpMethodEnum.Patch)]
        public async Task<IHttpActionResult> Patch()
        {
            return await this.Execute();
        }

        /// <summary>
        /// Handles Post method
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        [AcceptVerbs(HttpMethodEnum.Post)]
        public async Task<IHttpActionResult> Post()
        {
            return await this.Execute();
        }

        /// <summary>
        /// Handles Put method
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        [AcceptVerbs(HttpMethodEnum.Put)]
        public async Task<IHttpActionResult> Put()
        {
            return await this.Execute();
        }


        /// <summary>
        /// Method which handles all incomming request and resolves which
        /// response will be returned to the client.
        /// It accepts 'all' common HTTP methods
        /// </summary>
        /// <returns>HttpResponseMessage instance</returns>
        /// 
        private async Task<IHttpActionResult> Execute()
        {
            var resourceUri = this.Request.RequestUri.AbsolutePath.ToLower();
            var method = this.Request.Method.Method;
            var conf = IocFactory.Instance.Configuration.GetConfigurationByResource(resourceUri, method);

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
