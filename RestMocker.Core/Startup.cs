using System;
using System.Web.Http;
using Owin;
using RestMocker.Core.Services;
using RestMocker.Core.Utils;
using Swashbuckle.Application;

namespace RestMocker.Core
{
    /// <summary>
    /// OWIN startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            var config = IocFactory.Instance.HttpConfiguration;
            var logger = IocFactory.Instance.Logger;

            config.EnableSwagger(c => c.SingleApiVersion("v1", "RestMocker API"))
                .EnableSwaggerUi();

            //Config controller
            config.Routes.MapHttpRoute(
                name: "Config",
                routeTemplate: ConfigurationService.ConfigRouteTemplate,
                defaults: new { controller = "Config" }
            );


            config.Filters.Add(new ExceptionHandlingAttribute());
            config.Filters.Add(new ValidateModelAttribute());


            logger.Info("Configuration loaded");
            app.UseWebApi(config);
        }
    }
}
