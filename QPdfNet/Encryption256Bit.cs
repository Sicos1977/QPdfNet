//
// Encryption256Bit.cs
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

using Newtonsoft.Json;
using QPdfNet.Enums;
using QPdfNet.Interfaces;
using QPdfNet.Loggers;

namespace QPdfNet
{
    /// <summary>
    ///     Use 256 bit encryption
    /// </summary>
    public class Encryption256Bit : IEncryption
    {
        #region Fields
        [JsonProperty("accessibility")] private string _accessibility;
        [JsonProperty("annotate")] private string _annotate;
        [JsonProperty("assemble")] private string _assemble;
        [JsonProperty("extract")] private string _extract;
        [JsonProperty("form")] private string _form;
        [JsonProperty("modifyOther")] private string _modifyOther;
        [JsonProperty("modify")] private Modify _modify;
        [JsonProperty("print")] private Print _print;
        [JsonProperty("cleartextMetadata")] private string? _cleartextMetadata;
        #endregion

        #region Constructor
        /// <summary>
        ///      Encrypt the PDF with 256 bit encryption
        /// </summary>
        /// <param name="accessibility">Restrict accessibility (usually ignored)</param>
        /// <param name="annotate">Restrict commenting/filling form fields</param>
        /// <param name="assemble">Restrict document assembly</param>
        /// <param name="extract">Restrict text/graphic extraction</param>
        /// <param name="form">Restrict filling form fields</param>
        /// <param name="modifyOther">Restrict other modifications</param>
        /// <param name="modify">Control <see cref="_modify"/> access by level</param>
        /// <param name="print">Control printing access</param>
        /// <param name="cleartextMetaData">Prevent encryption of metadata</param>
        public Encryption256Bit(
            bool accessibility = true, 
            bool annotate = true, 
            bool assemble = true, 
            bool extract = true, 
            bool form = true, 
            bool modifyOther = true,
            Modify modify = Modify.All,
            Print print = Print.Full,
            bool cleartextMetaData = false)
        {
            Logger.LogInformation($"Encryption options accessibility '{accessibility}', annotate '{annotate}', assemble '{assemble}'" +
                                  $", extract '{extract}', form '{form}', modify other '{modifyOther}', modify '{modify}'" +
                                  $", print '{print}' and cleartext meta-data '{cleartextMetaData}'");


            _accessibility = accessibility ? "y" : "n";
            _annotate = annotate ? "y" : "n";
            _assemble = assemble ? "y" : "n";
            _extract = extract ? "y" : "n";
            _form = form ? "y" : "n";
            _modifyOther = modifyOther ? "y" : "n";
            _modify = modify;
            _print = print;

            if (cleartextMetaData)
                _cleartextMetadata = string.Empty;
        }
    }
    #endregion
}
