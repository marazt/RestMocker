using System;
using System.Collections.Generic;


namespace RestMocker.ConfigTransformer
{
    /// <summary>
    /// Main class
    /// </summary>
    class Program
    {

        /// <summary>
        /// The transformers
        /// </summary>
        private static readonly Dictionary<string, ITransformer> Transformers = new Dictionary<string, ITransformer>();
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            var options = new ArgOptions();
            var result = CommandLine.Parser.Default.ParseArguments(args, options);
            if (!result)
            {
                Console.WriteLine("Invalid arguments set");
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(options).ToString());
                return;
            }

            AddTransformers();
            ITransformer transformer;

            if (!Transformers.TryGetValue(options.SourceType, out transformer))
            {
                Console.WriteLine("Transformer '{0}' not found", options.SourceType);
            }
            else
            {
                Console.WriteLine("Transforming with '{0}'", transformer.Name);
                transformer.Trasform(options.SourcePath, options.TargetPath);
                Console.WriteLine("Transformation save into file '{0}'", options.TargetPath);
            }

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();

        }


        /// <summary>
        /// Adds the transformers helper.
        /// </summary>
        /// <param name="transformer">The transformer.</param>
        private static void AddTransformersHelper(ITransformer transformer)
        {
            Transformers.Add(transformer.Name, transformer);
        }

        /// <summary>
        /// Adds the transformers.
        /// </summary>
        private static void AddTransformers()
        {
            AddTransformersHelper(new SwaggerTransformer12());
        }

        /// <summary>
        /// Unhandled exception handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("Sorry, there was a critical error, press eny key to exit");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
