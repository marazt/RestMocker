using CommandLine;

namespace RestMocker.ConfigTransformer
{
    /// <summary>
    /// CommandLine parser options
    /// </summary>
    public class ArgOptions
    {
        /// <summary>
        /// Gets or sets the type of the source.
        /// </summary>
        /// <value>
        /// The type of the source.
        /// </value>
        [Option('v', "version", Required = true, HelpText = "Source version (swagger1.2)")]
        public string SourceType { get; set; }

        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        /// <value>
        /// The source path.
        /// </value>
        [Option('s', "source", Required = true, HelpText = "Source file path")]
        public string SourcePath { get; set; }

        /// <summary>
        /// Gets or sets the target path.
        /// </summary>
        /// <value>
        /// The target path.
        /// </value>
        [Option('t', "target", Required = true, HelpText = "Source file path")]
        public string TargetPath { get; set; }

    }
}

