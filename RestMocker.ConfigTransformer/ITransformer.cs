
namespace RestMocker.ConfigTransformer
{
    /// <summary>
    /// Transformer interface
    /// </summary>
    public interface ITransformer
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }
        /// <summary>
        /// Trasforms the specified source path.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="targetPath">The target path.</param>
        void Trasform(string sourcePath, string targetPath);
    }
}
