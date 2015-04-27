using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace RestMocker.Model
{
    /// <summary>
    /// Configuration Response item
    /// </summary>
    public class ResponseItem
    {

        #region Properties


        ///// <summary>
        ///// Gets or sets the json schema.
        ///// </summary>
        ///// <value>
        ///// The json schema.
        ///// </value>
        //[JsonProperty(Required = Required.AllowNull)]
        //public Dictionary<string, string> JsonSchema { get; set; }

        /// <summary>
        /// Gets or sets the json.
        /// </summary>
        /// <value>
        /// The json.
        /// </value>
        [JsonProperty(Required = Required.AllowNull)]
        public Dictionary<string, string> Json { get; set; }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        [JsonProperty(Required = Required.AllowNull)]
        public Dictionary<string, string> Headers { get; set; }

        ///// <summary>
        ///// Gets or sets the data.
        ///// </summary>
        ///// <value>
        ///// The data.
        ///// </value>
        //[JsonProperty(Required = Required.AllowNull)]
        //public string Data { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        [JsonProperty(Required = Required.Always)]
        public HttpStatusCode StatusCode { get; set; }


        #endregion Properties



        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseItem"/> class.
        /// </summary>
        public ResponseItem()
        {
            //this.Data = string.Empty;
            this.StatusCode = HttpStatusCode.OK;
            this.Headers = new Dictionary<string, string>();
            this.Json = new Dictionary<string, string>();
            //this.JsonSchema = new Dictionary<string, string>();
        }

        #endregion Constructors
    }
}