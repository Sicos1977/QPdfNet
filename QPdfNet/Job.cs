//
// Class1.cs
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
using QPdfNet.Interop;

namespace QPdfNet
{
    /// <summary>
    ///     Represent a qpdf job
    /// </summary>
    /// <remarks>
    ///     https://qpdf.readthedocs.io/en/stable/qpdf-job.html
    /// </remarks>
    public class Job
    {
        #region Fields
        [JsonProperty("inputFile")]
        private string _inputFile;

        [JsonProperty("password")]
        private string _password;

        [JsonProperty("outputFile")]
        private string _outputFile;

        [JsonProperty("encrypt")]
        private Encryption _encryption;

        [JsonProperty("linearize")]
        private string _linearize;
        #endregion

        /// <summary>
        ///     The input PDF file
        /// </summary>
        /// <param name="fileName">The input file</param>
        /// <param name="password">The password to open the file or <c>null</c></param>
        /// <returns></returns>
        public Job InputFile(string fileName, string password = null)
        {
            _inputFile = fileName;
            _password = password;
            return this;
        }

        /// <summary>
        ///     The output PDF file
        /// </summary>
        /// <param name="fileName">The output file</param>
        /// <returns></returns>
        public Job OutputFile(string fileName)
        {
            _outputFile = fileName;
            return this;
        }

        /// <summary>
        ///     Generate a secure, random document ID using deterministic values.
        ///     This prevents use of timestamp and output file name information in the ID generation.
        ///     Instead, at some slight additional runtime cost, the ID field is generated to include a digest of the significant parts of the content of the output PDF file. This means that a given qpdf operation should generate the same ID each time it is run, which can be useful when caching results or for generation of some test data. Use of this flag is not compatible with creation of encrypted files. This option can be useful for testing
        /// </summary>
        /// <returns></returns>
        public Job WithLinearize()
        {
            _linearize = string.Empty;
            return this;
        }


        /// <summary>
        ///     The encryption options to use
        /// </summary>
        /// <remarks>
        ///     If no <paramref name="options"/> are given then 256 bits encryption is used
        /// </remarks>
        /// <returns></returns>
        public Job Encrypt(string userPassword, string ownerPassword, EncryptionOptions options)
        {
            _encryption = new Encryption(userPassword, ownerPassword, options);
            return this;
        }

        /// <summary>
        ///     Runs the job with the given parameters
        /// </summary>
        public ExitCodes Run()
        {
            var settings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore };
            var json = JsonConvert.SerializeObject(this, settings);
            return QPdfApi.Native.RunFromJSON(json);
        }
    }
}
