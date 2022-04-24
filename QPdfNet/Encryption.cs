//
// Encryption.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright (c) 2021-2022 Kees van Spelde.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

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
        [JsonProperty("40bit")] private IEncryption? _options40;
        [JsonProperty("128bit")] private IEncryption? _options128;
        [JsonProperty("256bit")] private IEncryption? _options256;
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
