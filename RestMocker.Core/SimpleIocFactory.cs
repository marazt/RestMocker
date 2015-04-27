using System.Web.Http;
using NLog;
using RestMocker.Core.Services;

namespace RestMocker.Core
{
    /// <summary>
    /// Simple IoC Factory (instance holder) isntead of using complex IoCs like Castle or Unity
    /// Can be replaced in the future, but now it is not needed :)
    /// </summary>
    public class SimpleIocFactory
    {
        #region Attributes

        /// <summary>
        /// The instance
        /// </summary>
        private static SimpleIocFactory instance;

        #endregion Attributes

        #region Properties
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static SimpleIocFactory Instance
        {
            get { return instance ?? (instance = new SimpleIocFactory()); }
        }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfigurationService Configuration { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public Logger Logger { get; set; }


        /// <summary>
        /// Gets or sets the HttpConfiguration.
        /// </summary>
        /// <value>
        /// The HttpConfiguration.
        /// </value>
        public HttpConfiguration HttpConfiguration { get; set; }

        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleIocFactory"/> class.
        /// </summary>
        private SimpleIocFactory()
        {
            this.HttpConfiguration = new HttpConfiguration();
            this.Configuration = new ConfigurationService(this.HttpConfiguration);
            this.Logger = LogManager.GetLogger("*");
        }

        #endregion Constructors
    }
}
