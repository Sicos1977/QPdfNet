using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QPdfNet.Enums;

namespace QPdfNet
{
    /// <summary>
    ///     The encryption settings
    /// </summary>
    public class EncryptionOptions
    {
        #region Properties
        /// <summary>
        ///  Printing options
        /// </summary>
        [JsonProperty("print")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EncryptionPrint Print { get; set; }

        /// <summary>
        ///     Modification options
        /// </summary>
        [JsonProperty("modify")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EncryptionModify Modify { get; set; }
        #endregion
    }
}
