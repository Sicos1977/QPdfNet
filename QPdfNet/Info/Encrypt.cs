using Newtonsoft.Json;

namespace QPdfNet.Info
{
    /// <summary>
    ///     Returns information about the PDF encryption
    /// </summary>
    public class Encrypt
    {
        [JsonProperty("capabilities")]
        public Capabilities? Capabilities { get; private set; }

        [JsonProperty("encrypted")]
        public bool Encrypted { get; private set; }

        [JsonProperty("ownerpasswordmatched")]
        public bool OwnerPasswordMatched { get; private set; }

        [JsonProperty("parameters")]
        public EncryptParameters? Parameters { get; private set; }

        [JsonProperty("userpasswordmatched")]
        public bool UserPasswordMatched { get; private set; }
    }
}
