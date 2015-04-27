using System.ComponentModel;
using Newtonsoft.Json;

namespace RestMocker.Model
{
    /// <summary>
    /// Configuration item
    /// </summary>
    public class JsonConfigurationItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        /// <value>
        /// The resource.
        /// </value>
        [JsonProperty(Required = Required.Always)]
        public string Resource { get; set; }

        /// <summary>
        /// Http method
        /// </summary>
        private string method;

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        [JsonProperty(Required = Required.Always)]
        public string Method
        {
            get { return this.method; }
            set { this.method = value != null ? value.ToLower() : null; }
        }


        /// <summary>
        /// Gets or sets the minimum delay of the response.
        /// </summary>
        /// <value>
        /// The minimum delay.
        /// </value>
        [DefaultValue(0)]
        [JsonProperty(Required = Required.Default)]
        public int MinDelay { get; set; }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>
        /// The response.
        /// </value>
        [JsonProperty(Required = Required.Always)]
        public ResponseItem Response { get; set; }


        #endregion Properties


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigurationItem"/> class.
        /// </summary>
        public JsonConfigurationItem()
        {
            this.Response = new ResponseItem();
        }

        #endregion Constructors

    }



}