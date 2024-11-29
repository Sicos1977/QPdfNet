//
// Encryption40Bit.cs
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
using QPdfNet.Interfaces;

namespace QPdfNet
{
    /// <summary>
    ///     Use 40 bit encryption
    /// </summary>
    public class Encryption40Bit : IEncryption
    {
        #region Fields
        [JsonProperty("annotate")]
        private string _annotate;

        [JsonProperty("extract")]
        private string _extract;

        [JsonProperty("modify")]
        private Modify _modify;

        [JsonProperty("print")]
        private Print _print;
        #endregion

        #region Constructor
        /// <summary>
        ///     Encrypt the PDF with 40 bit encryption
        /// </summary>
        /// <param name="annotate">Restrict comments, filling forms, and signing</param>
        /// <param name="extract">Restrict text/graphic extraction</param>
        /// <param name="modify">Restrict document <see cref="Modify"/></param>
        /// <param name="print">Restrict <see cref="Print"/></param>
        public Encryption40Bit(
            bool annotate = true, 
            bool extract = true, 
            Modify modify = Modify.All, 
            Print print = Print.Full)
        {
            _annotate = annotate ? "y" : "n";
            _extract = extract ? "y" : "n";
            _modify = modify;
            _print = print;
        }
        #endregion
    }
}
