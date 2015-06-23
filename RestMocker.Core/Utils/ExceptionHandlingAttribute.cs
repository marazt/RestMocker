using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace RestMocker.Core.Utils
{
    /// <summary>
    /// Ecxpetion handler for WebApi requests
    /// </summary>
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Called when [exception].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        /// <exception cref="HttpResponseMessage"></exception>
        /// <exception cref="StringContent">An error occurred, please try again or contact the administrator.</exception>
        public override void OnException(HttpActionExecutedContext context)
        {
            //Log Critical errors
            IocFactory.Instance.Logger.ErrorException("Appliction Exception", context.Exception);

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An error occurred, please try again or contact the administrator."),
                ReasonPhrase = "Critical Exception"
            });
        }
    }
}