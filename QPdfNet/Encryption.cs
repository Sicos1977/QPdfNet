using System;
using Newtonsoft.Json;
using QPdfNet.Interfaces;

namespace QPdfNet
{
    /// <summary>
    ///     The encryption settings
    /// </summary>
    internal class Encryption
    {
        #region Fields
        /// <summary>
        ///     The encryption options
        /// </summary>
        [JsonProperty("40bit")]
        private IEncryption _options40;

        [JsonProperty("128bit")]
        private IEncryption _options128;

        [JsonProperty("256bit")]
        private IEncryption _options256;
        #endregion

        #region Properties
        /// <summary>
        ///     The user password
        /// </summary>
        [JsonProperty("userPassword")]
        public string UserPassword { get; private set; }

        /// <summary>
        ///     The owner password
        /// </summary>
        [JsonProperty("ownerPassword")]
        public string OwnerPassword { get; private set; }

        [JsonIgnore]
        public IEncryption Options;
        #endregion

        #region Constructor
        /// <summary>
        ///     Makes this object and sets its needed properties
        /// </summary>
        /// <param name="userPassword">The user password</param>
        /// <param name="ownerPassword">The owner password</param>
        /// <param name="options"><see cref="IEncryption"/></param>
        internal Encryption(
            string userPassword, 
            string ownerPassword,
            IEncryption options)
        {
            UserPassword = userPassword;
            OwnerPassword = ownerPassword;

            switch (options.GetType().Name)
            {
                case "Encryption40Bit":
                    _options40 = options;
                    Options = _options40;
                    break;

                case "Encryption128Bit":
                    _options128 = options;
                    Options = _options128;
                    break;

                case "Encryption256Bit":
                    _options256 = options;
                    Options = _options256;
                    break;

                default:
                    throw new ArgumentException("User either the class Encryption40Bit, Encryption128Bit or Encryption256Bit");
            }
        }
        #endregion
    }
}
