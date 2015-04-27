using System;
using System.Configuration;

namespace RestMocker.Console
{
    /// <summary>
    /// Custom congif helper class
    /// </summary>
    public static class AppConfig
    {
        # region Constants

        private const string PortKey = "port";
        private const string HostKey = "host";

        # endregion Constants

        # region Properties

        /// <summary>
        /// Get post on which run
        /// </summary>
        public static readonly int Port = Int32.Parse(ConfigurationManager.AppSettings[PortKey]);

        /// <summary>
        /// Gets host where to run
        /// </summary>
        public static readonly string Host = ConfigurationManager.AppSettings[HostKey];

        #endregion Properties
    }
}
