using System.Web.Http;
using Ninject;
using Ninject.Extensions.Logging;
using Ninject.Extensions.Logging.NLog3;
using Ninject.Parameters;
using RestMocker.Core.Services;

namespace RestMocker.Core
{
    /// <summary>
    /// IoC Factory
    /// </summary>
    public class IocFactory
    {
        #region Attributes

        /// <summary>
        /// The instance
        /// </summary>
        private static IocFactory instance;

        private IKernel container;

        #endregion Attributes

        #region Properties
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static IocFactory Instance
        {
            get { return instance ?? (instance = new IocFactory()); }
        }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfigurationService Configuration
        {
            get { return this.Resolve<IConfigurationService>(); }
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger Logger
        {
            get { return this.Resolve<ILoggerFactory>().GetLogger("*"); }
        }


        /// <summary>
        /// Gets or sets the HttpConfiguration.
        /// </summary>
        /// <value>
        /// The HttpConfiguration.
        /// </value>
        public HttpConfiguration HttpConfiguration
        {
            get { return this.Resolve<HttpConfiguration>(); }
        }

        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="IocFactory"/> class.
        /// </summary>
        private IocFactory()
        {
            var settings = new NinjectSettings() { LoadExtensions = false };
            this.container = new StandardKernel(settings, new NLogModule());
            this.container.Bind<HttpConfiguration>().To<HttpConfiguration>().InSingletonScope();
            this.container.Bind<IConfigurationService>()
                .To<ConfigurationService>()
                .InSingletonScope()
                .WithConstructorArgument(new ConstructorArgument("configurationFile", "Configuration/config.json"));
        }

        /// <summary>
        /// Registers the specified inst.
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="inst">The inst.</param>
        public void Register<I, T>(T inst)
            where I : class
            where T : I
        {
            this.container.Rebind<I>().ToConstant(inst);
        }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="T"></typeparam>
        public void Register<I, T>()
            where I : class
            where T : I
        {
            this.container.Rebind<I>().To<T>();
        }

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <returns>Instance of I</returns>
        public I Resolve<I>() where I : class
        {
            return this.container.Get<I>();
        }

        #endregion Constructors
    }
}
