using System;
using Microsoft.Owin.Hosting;
using RestMocker.Core;

namespace RestMocker.Console
{
    /// <summary>
    /// Main class
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            var baseUri = string.Format("{0}:{1}", AppConfig.Host, AppConfig.Port);
            // Start OWIN host 
            WebApp.Start<Startup>(url: baseUri);
            System.Console.WriteLine("Listening on '{0}'", baseUri);
            System.Console.ReadLine();
        }

        /// <summary>
        /// Unhandled exception handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            IocFactory.Instance.Logger.Fatal(string.Format("Fatal exception: {0}", e.ExceptionObject));
            System.Console.WriteLine("Sorry, there was a critical error, press eny key to exit");
            System.Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
