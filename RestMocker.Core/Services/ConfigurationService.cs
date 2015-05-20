using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Ninject.Extensions.Logging;
using RestMocker.Model;

namespace RestMocker.Core.Services
{
    /// <summary>
    /// Configuration service class
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        #region Constants

        private static readonly object ConfigLock = new object();
        public const string ConfigRouteTemplate = "restmocker/config";

        #endregion Constants

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationService"/> class.
        /// </summary>
        public ConfigurationService(ILogger logger, HttpConfiguration httpConfiguration)
        {
            this.logger = logger;
            this.HttpConfiguration = httpConfiguration;
            this.Configurations = new List<JsonConfigurationItem>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Gets the configurations.
        /// </summary>
        /// <value>
        /// The configurations.
        /// </value>
        private List<JsonConfigurationItem> Configurations { get; set; }

        /// <summary>
        /// Gets or sets the HttpConfiguration.
        /// </summary>
        /// <value>
        /// The HttpConfiguration.
        /// </value>
        public HttpConfiguration HttpConfiguration { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <param name="configurationFilePath">The configuration file path.</param>
        public void LoadConfiguration(string configurationFilePath)
        {
            if (!File.Exists(configurationFilePath))
            {
               this.logger.Warn("Configuration file '{0}' not found", configurationFilePath);
                return;
            }

            using (var r = new StreamReader(configurationFilePath))
            {
                var json = r.ReadToEnd();
                this.SetConfiguration(JsonConvert.DeserializeObject<List<JsonConfigurationItem>>(json,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Error,
                        DefaultValueHandling = DefaultValueHandling.Include
                    }));
            }
        }

        /// <summary>
        /// Gets the configuration by resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="method">Http method.</param>
        /// <returns>JsonConfigurationItem instance if found, otherwise null</returns>
        public JsonConfigurationItem GetConfigurationByResource(string resource, string method)
        {
            var separator = new[] { '/' };
            method = method.ToLower();
            resource = resource.Substring(1); // remove '/'
            var item = this.Configurations.FirstOrDefault(e => e.Resource.Equals(resource.ToLower()));
            if (item != null && item.Method == method)
            {
                return item;
            }

            var resourceParts = resource.Split(separator);
            JsonConfigurationItem result = null;

            Parallel.ForEach(this.Configurations, conf =>
                                                  {
                                                      var confParts = conf.Resource.Split(separator);
                                                      var res = this.TryMatchResources(confParts, resourceParts);
                                                      if (res && conf.Method == method)
                                                      {
                                                          result = conf;
                                                          return;
                                                      }
                                                  });

            return result;
        }


        /// <summary>
        /// Helper method try match set resource and resource defined in the config file
        /// </summary>
        /// <param name="pattern">Resource from configuration splitted by '/'</param>
        /// <param name="resource">Resource from controller request '/'</param>
        /// <returns></returns>
        private bool TryMatchResources(IList<string> pattern, IList<string> resource)
        {
            var regex = new Regex(@"\{.+\}", RegexOptions.IgnoreCase);
            if (pattern.Count != resource.Count)
            {
                return false;
            }

            for (var i = 0; i < pattern.Count; ++i)
            {
                var pat = pattern[i];
                var res = resource[i];

                if (pat == res)
                {
                    continue;
                }

                var match = regex.Match(pat);
                if (!match.Success)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gest the configuration
        /// </summary>
        /// <returns>Collection of configurations</returns>
        public List<JsonConfigurationItem> GetConfiguration()
        {
            lock (ConfigLock)
            {
                return this.Configurations.ToList();
            }
        }


        /// <summary>
        /// Sets the configuration
        /// </summary>
        /// <param name="configuration"></param>
        public void SetConfiguration(IEnumerable<JsonConfigurationItem> configuration)
        {
            lock (ConfigLock)
            {
                this.Configurations.Clear();
                this.Configurations.AddRange(configuration);
                this.ApplyConfiguration();
            }
        }

        /// <summary>
        /// Method to apply configuration into routes
        /// </summary>
        private void ApplyConfiguration()
        {
            // remove old configuration
            foreach (var route in this.HttpConfiguration.Routes.Where(e => e.RouteTemplate != ConfigRouteTemplate).ToList())
            {
                this.HttpConfiguration.Routes.Remove(route.RouteTemplate);
            }

            // set new
            foreach (var conf in this.Configurations)
            {
                this.HttpConfiguration.Routes.MapHttpRoute(conf.Name, conf.Resource, defaults: new { controller = "Common", action = "Execute" });
            }
        }

        #endregion Methods
    }
}
