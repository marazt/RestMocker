using System.Collections.Generic;
using RestMocker.Model;

namespace RestMocker.Core.Services
{
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets the configuration
        /// </summary>
        /// <returns></returns>
        List<JsonConfigurationItem> GetConfiguration();

        /// <summary>
        /// Set configuration
        /// </summary>
        /// <param name="configuration"></param>
        void SetConfiguration(IEnumerable<JsonConfigurationItem> configuration);

        /// <summary>
        /// Gets the configuration by resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="method">Http method.</param>
        /// <returns>JsonConfigurationItem instance if found, otherwise null</returns>
        JsonConfigurationItem GetConfigurationByResource(string resource, string method);
    }
}
