//
// Encryption128Bit.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright (c) 2021-2024 Kees van Spelde.
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

using Newtonsoft.Json;
using QPdfNet.Enums;
using QPdfNet.Loggers;

namespace QPdfNet
{
    /// <summary>
    ///     Use 128 bit encryption
    /// </summary>
    public class Encryption128Bit : Encryption256Bit
    {
        #region Fields
        [JsonProperty("useAes")] private string _useAes;
        #endregion

        #region Constructor
        /// <summary>
        ///      Encrypt the PDF with 128 bit encryption
        /// </summary>
        /// <param name="accessibility">Restrict accessibility (usually ignored)</param>
        /// <param name="annotate">Restrict commenting/filling form fields</param>
        /// <param name="assemble">Restrict document assembly</param>
        /// <param name="extract">Restrict text/graphic extraction</param>
        /// <param name="form">Restrict filling form fields</param>
        /// <param name="modifyOther">Restrict other modifications</param>
        /// <param name="modify">Control <see cref="Enums.Modify"/> access by level</param>
        /// <param name="print">Control printing access</param>
        /// <param name="cleartextMetaData">Prevent encryption of metadata</param>
        /// <param name="useAes">Use AES instead of RC4 encryption</param>
        public Encryption128Bit(
            bool accessibility = true,
            bool annotate = true,
            bool assemble = true,
            bool extract = true,
            bool form = true,
            bool modifyOther = true,
            Modify modify = Modify.All,
            Print print = Print.Full,
            bool cleartextMetaData = false,
            bool useAes = false) :
            base(
                accessibility,
                annotate,
                assemble,
                extract,
                form,
                modifyOther,
                modify,
                print, 
                cleartextMetaData)
        {
            Logger.LogInformation($"128-bit encryption is using {(useAes ? "AES" : "RC4")} encryption");

            _useAes = useAes ? "y" : "n";
        }
        #endregion
    }
}
