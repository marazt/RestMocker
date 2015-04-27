using System;
using System.Web.Http;
using Owin;
using RestMocker.Core.Services;
using RestMocker.Core.Utils;

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
            var config = SimpleIocFactory.Instance.HttpConfiguration;

            //Config controller
            config.Routes.MapHttpRoute(
                name: "Config",
                routeTemplate: ConfigurationService.ConfigRouteTemplate,
                defaults: new { controller = "Config" }
            );

            const string configurationFile = "Configuration/config.json";
            SimpleIocFactory.Instance.Logger.Info("Loading configuration from '{0}'", configurationFile);
            try
            {
                SimpleIocFactory.Instance.Configuration.LoadConfiguration(configurationFile);
                config.Filters.Add(new ExceptionHandlingAttribute());
            }
            catch (Exception ex)
            {
                SimpleIocFactory.Instance.Logger.ErrorException("Error while loading configuration", ex);
                throw ex;
            }

            SimpleIocFactory.Instance.Logger.Info("Configuration loaded");
            app.UseWebApi(config);
        }
    }
}
