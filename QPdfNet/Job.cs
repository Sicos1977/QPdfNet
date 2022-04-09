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

using System.IO;
using Newtonsoft.Json;
using QPdfNet.Enums;
using QPdfNet.Interop;

namespace QPdfNet;

/// <summary>
///     Represent a qpdf job
/// </summary>
/// <remarks>
///     https://qpdf.readthedocs.io/en/stable/qpdf-job.html
/// </remarks>
public class Job
{
    #region Fields
    [JsonProperty("inputFile")] private string _inputFile;

    [JsonProperty("outputFile")] private string _outputFile;
    [JsonProperty("replaceInput")] private string _replaceInput;
    [JsonProperty("warningExit0")] private string _warningExit0;
    [JsonProperty("password")] private string _password;
    [JsonProperty("passwordFile")] private string _passwordFile;
    [JsonProperty("noWarn")] private string _noWarn;
    [JsonProperty("deterministicId")] private string _deterministicId;
    [JsonProperty("allowWeakCrypto")] private string _allowWeakCrypto;
    [JsonProperty("passwordIsHexKey")] private string _passwordIsHexKey;

    [JsonProperty("encrypt")] private Encryption _encryption;
    [JsonProperty("linearize")] private string _linearize;
    [JsonProperty("splitPages")] private string _splitPages;
    #endregion

    #region InputFile
    /// <summary>
    ///     The input PDF file
    /// </summary>
    /// <param name="fileName">The input file</param>
    /// <param name="password">The password to open the file or <c>null</c></param>
    /// <returns><see cref="Job"/></returns>
    public Job InputFile(string fileName, string password = null)
    {
        _inputFile = fileName;
        _password = password;
        return this;
    }
    #endregion

    #region OutputFile
    /// <summary>
    ///     The output PDF file
    /// </summary>
    /// <param name="fileName">The output file</param>
    /// <returns><see cref="Job"/></returns>
    public Job OutputFile(string fileName)
    {
        _outputFile = fileName;
        return this;
    }
    #endregion

    #region ReplaceInput
    /// <summary>
    ///     This option may be given in place of outfile. This causes qpdf to replace the input file with the output.
    ///     It does this by writing to infilename.~qpdf-temp# and, when done, overwriting the input file with the temporary
    ///     file. If there were any warnings, the original input is saved as infilename.~qpdf-orig. If there are errors, the
    ///     input file is left untouched.
    /// </summary>
    /// <returns><see cref="Job"/></returns>
    public Job ReplaceInput()
    {
        _replaceInput = string.Empty;
        return this;
    }
    #endregion

    #region WarningExit0
    /// <summary>
    ///     If there were warnings only and no errors, exit with exit code 0 instead of 3. When combined with
    ///     <see cref="NoWarn" />, the effect
    ///     is for qpdf to completely ignore warnings.
    /// </summary>
    /// <returns><see cref="Job"/></returns>
    public Job WarningExit0()
    {
        _warningExit0 = string.Empty;
        return this;
    }
    #endregion

    #region Password
    /// <summary>
    ///     Specifies a password for accessing encrypted, password-protected files. To read the password from a file or
    ///     standard input
    /// </summary>
    /// <returns><see cref="Job"/></returns>
    public Job Password()
    {
        _password = string.Empty;
        return this;
    }
    #endregion

    #region PasswordFile
    /// <summary>
    ///     Reads the first line from the specified file and uses it as the password for accessing encrypted files
    /// </summary>
    /// <param name="fileName">The file with full path</param>
    /// <returns><see cref="Job"/></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public Job PasswordFile(string fileName)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException(fileName);

        _passwordFile = string.Empty;
        return this;
    }
    #endregion

    #region NoWarn
    /// <summary>
    ///     Suppress writing of warnings to stderr. If warnings were detected and suppressed, qpdf will still exit with exit
    ///     code 3. To completely ignore warnings, also specify --warning-exit-0. Use with caution as qpdf is not always
    ///     successful in recovering from situations that cause warnings to be issued.
    /// </summary>
    /// <returns><see cref="Job"/></returns>
    public Job NoWarn()
    {
        _noWarn = string.Empty;
        return this;
    }
    #endregion

    #region DeterministicId
    /// <summary>
    ///     Generate a secure, random document ID using deterministic values. This prevents use of timestamp and output file
    ///     name information in the ID generation. Instead, at some slight additional runtime cost, the ID field is generated
    ///     to include a digest of the significant parts of the content of the output PDF file. This meanGenerate a secure,
    ///     random document ID using deterministic values. This prevents use of timestamp and output file name information in
    ///     the ID generation. Instead, at some slight additional runtime cost, the ID field is generated to include a digest
    ///     of the significant parts of the content of the output PDF file. This means that a given qpdf operation should
    ///     generate the same ID each time it is run, which can be useful when caching results or for generation of some test
    ///     data. Use of this flag is not compatible with creation of encrypted files. This option can be useful for testing.
    ///     See also --static-id.
    /// </summary>
    /// <returns><see cref="Job"/></returns>
    public Job DeterministicId()
    {
        _deterministicId = string.Empty;
        return this;
    }
    #endregion

    #region AllowWeakCrypto
    /// <summary>
    /// Starting with version 10.4, qpdf issues warnings when requested to create files using RC4 encryption. This option suppresses those warnings. In future versions of qpdf, qpdf will refuse to create files with weak cryptography when this flag is not given. See Weak Cryptography for additional details.
    /// </summary>
    /// <returns><see cref="Job"/></returns>
    public Job AllowWeakCrypto()
    {
        _allowWeakCrypto = string.Empty;
        return this;
    }
    #endregion

    #region PasswordIsHexKey
    /// <summary>
    /// Overrides the usual computation/retrieval of the PDF file’s encryption key from user/owner password with an explicit specification of the encryption key. When this option is specified, the parameter to the --password option is interpreted as a hexadecimal-encoded key value. This only applies to the password used to open the main input file. It does not apply to other files opened by --pages or other options or to files being written.
    /// Most users will never have a need for this option, and no standard viewers support this mode of operation, but it can be useful for forensic or investigatory purposes. For example, if a PDF file is encrypted with an unknown password, a brute-force attack using the key directly is sometimes more efficient than one using the password.Also, if a file is heavily damaged, it may be possible to derive the encryption key and recover parts of the file using it directly.To expose the encryption key used by an encrypted file that you can open normally, use the --show-encryption-key option.
    /// </summary>
    /// <returns><see cref="Job"/></returns>
    public Job PasswordIsHexKey()
    {
        _passwordIsHexKey = string.Empty;
        return this;
    }
    #endregion

    #region WithLinearize
    /// <summary>
    ///     Generate a secure, random document ID using deterministic values.
    ///     This prevents use of timestamp and output file name information in the ID generation.
    ///     Instead, at some slight additional runtime cost, the ID field is generated to include a digest of the significant
    ///     parts of the content of the output PDF file. This means that a given qpdf operation should generate the same ID
    ///     each time it is run, which can be useful when caching results or for generation of some test data. Use of this flag
    ///     is not compatible with creation of encrypted files. This option can be useful for testing
    /// </summary>
    /// <returns><see cref="Job"/></returns>
    public Job WithLinearize()
    {
        _linearize = string.Empty;
        return this;
    }
    #endregion

    #region SplitPages
    /// <summary>
    ///     Write each group of <paramref name="n" /> pages to a separate  <see cref="OutputFile" />. If <paramref name="n" />
    ///     is not specified, create single pages. Output file names are generated as follows:
    ///     If the string %d appears in the <see cref="OutputFile" /> name, it is replaced with a range of zero-padded page
    ///     numbers starting from 1.
    ///     Otherwise, if the output file name ends in .pdf(case insensitive), a zero-padded page range, preceded by a dash, is
    ///     inserted before the file extension.
    ///     Otherwise, the file name is appended with a zero-padded page range preceded by a dash.
    ///     Zero padding is added to all page numbers in file names so that all the numbers are the same length, which causes
    ///     the output filenames to sort lexically in numerical order.
    ///     Page ranges are a single number in the case of single-page groups or two numbers separated by a dash otherwise.for
    ///     testing
    /// </summary>
    /// <returns><see cref="Job"/></returns>
    public Job SplitPages(string n)
    {
        _splitPages = n;
        return this;
    }
    #endregion

    #region Encrypt
    /// <summary>
    ///     The encryption options to use
    /// </summary>
    /// <remarks>
    ///     If no <paramref name="options" /> are given then 256 bits encryption is used
    /// </remarks>
    /// <returns><see cref="Job"/></returns>
    public Job Encrypt(string userPassword, string ownerPassword, EncryptionOptions options)
    {
        _encryption = new Encryption(userPassword, ownerPassword, options);
        return this;
    }
    #endregion

    #region Run
    /// <summary>
    ///     Runs the job with the given parameters
    /// </summary>
    /// <returns><see cref="ExitCodes"/></returns>
    public ExitCodes Run()
    {
        var settings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore };
        var json = JsonConvert.SerializeObject(this, settings);
        return QPdfApi.Native.RunFromJSON(json);
    }
    #endregion
}