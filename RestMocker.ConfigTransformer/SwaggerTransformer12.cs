using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestMocker.Model;

namespace RestMocker.ConfigTransformer
{
    /// <summary>
    /// Transoformer for Swagger 1.2
    /// </summary>
    public class SwaggerTransformer12 : ITransformer
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return "swagger1.2"; }
        }
        /// <summary>
        /// Trasforms the specified source path.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="targetPath">The target path.</param>
        public void Trasform(string sourcePath, string targetPath)
        {
            var jsonConfigs = new List<JsonConfigurationItem>();
            var source = JObject.Parse(File.ReadAllText(sourcePath));
            var basePath = source["basePath"];

            foreach (var api in source["apis"])
            {
                var path = api["path"].Value<string>();

                foreach (var operation in api["operations"])
                {
                    var cfg = new JsonConfigurationItem
                    {
                        Name = path,
                        Resource = string.Format("{0}{1}", basePath, path),
                        MinDelay = 0,
                        Method = operation["method"].Value<string>(),
                        Response = new ResponseItem()
                    };

                    jsonConfigs.Add(cfg);
                }

            }

            using (var stream = new StreamWriter(targetPath))
            {
                stream.WriteLine(JsonConvert.SerializeObject(jsonConfigs,Formatting.Indented));
            }
        }
    }
}
