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
// ReSharper disable UnusedMember.Global

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

    [JsonProperty("suppressPasswordRecovery")]
    private string _suppressPasswordRecovery;

    [JsonProperty("passwordMode")] private PasswordMode _passwordMode;
    [JsonProperty("suppressRecovery")] private string _suppressRecovery;
    [JsonProperty("ignoreXrefStreams")] private string _ignoreXrefStreams;
    [JsonProperty("linearize")] private string _linearize;
    [JsonProperty("encrypt")] private Encryption _encryption;
    [JsonProperty("decrypt")] private string _decrypt;
    [JsonProperty("copyEncryption")] private string _copyEncryption;

    [JsonProperty("encryptionFilePassword")]
    private string _encryptionFilePassword;

    [JsonProperty("qpdf")] private string _qpdf;
    [JsonProperty("noOriginalObjectIds")] private string _noOriginalObjectIds;
    [JsonProperty("compressStreams")] private CompressStreams _compressStreams;
    [JsonProperty("decodeLevel")] private DecodeLevel _decodeLevel;
    [JsonProperty("splitPages")] private string _splitPages;
    #endregion

    #region InputFile
    /// <summary>
    ///     The input PDF file
    /// </summary>
    /// <param name="fileName">The input file</param>
    /// <param name="password">The password to open the file or <c>null</c></param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
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
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
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
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
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
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
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
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
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
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
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
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
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
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job DeterministicId()
    {
        _deterministicId = string.Empty;
        return this;
    }
    #endregion

    #region AllowWeakCrypto
    /// <summary>
    ///     Starting with version 10.4, qpdf issues warnings when requested to create files using RC4 encryption. This option
    ///     suppresses those warnings. In future versions of qpdf, qpdf will refuse to create files with weak cryptography when
    ///     this flag is not given. See Weak Cryptography for additional details.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job AllowWeakCrypto()
    {
        _allowWeakCrypto = string.Empty;
        return this;
    }
    #endregion

    #region PasswordIsHexKey
    /// <summary>
    ///     Overrides the usual computation/retrieval of the PDF file’s encryption key from user/owner password with an
    ///     explicit specification of the encryption key. When this option is specified, the parameter to the
    ///     <see cref="Password" /> option is interpreted as a hexadecimal-encoded key value. This only applies to the password
    ///     used to open the main input file. It does not apply to other files opened by --pages or other options or to files
    ///     being written.
    ///     Most users will never have a need for this option, and no standard viewers support this mode of operation, but it
    ///     can be useful for forensic or investigatory purposes. For example, if a PDF file is encrypted with an unknown
    ///     password, a brute-force attack using the key directly is sometimes more efficient than one using the password.Also,
    ///     if a file is heavily damaged, it may be possible to derive the encryption key and recover parts of the file using
    ///     it directly.To expose the encryption key used by an encrypted file that you can open normally, use the
    ///     <see cref="ShowEncryptionKey" /> option.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job PasswordIsHexKey()
    {
        _passwordIsHexKey = string.Empty;
        return this;
    }
    #endregion

    #region SuppressPasswordRecovery
    /// <summary>
    ///     Ordinarily, qpdf attempts to automatically compensate for passwords encoded with the wrong character encoding. This
    ///     option suppresses that behavior. Under normal conditions, there are no reasons to use this option. See Unicode
    ///     Passwords for a discussion.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job SuppressPasswordRecovery()
    {
        _suppressPasswordRecovery = string.Empty;
        return this;
    }
    #endregion

    #region PasswordMode
    /// <summary>
    ///     This option can be used to fine-tune how qpdf interprets Unicode (non-ASCII) password strings passed on the command
    ///     line. With the exception of the hex-bytes mode, these only apply to passwords provided when encrypting files. The
    ///     hex-bytes mode also applies to passwords specified for reading files. For additional discussion of the supported
    ///     password modes and when you might want to use them, see Unicode Passwords
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job PasswordMode(PasswordMode mode)
    {
        _passwordMode = mode;
        return this;
    }
    #endregion

    #region SuppressRecovery
    /// <summary>
    ///     Prevents qpdf from attempting to reconstruct a file’s cross reference table when there are errors reading objects
    ///     from the file. Recovery is triggered by a variety of situations. While usually successful, it uses heuristics that
    ///     don’t work on all files. If this option is given, qpdf fails on the first error it encounters.
    /// </summary>
    /// <see cref="Job" />
    public Job SuppressRecovery()
    {
        _suppressRecovery = string.Empty;
        return this;
    }
    #endregion

    #region IgnoreXrefStreams
    /// <summary>
    ///     Tells qpdf to ignore any cross-reference streams, falling back to any embedded cross-reference tables or triggering
    ///     document recovery. Ordinarily, qpdf reads cross-reference streams when they are present in a PDF file. If this
    ///     option is specified, qpdf will ignore any cross-reference streams for hybrid PDF files. The purpose of hybrid files
    ///     is to make some content available to viewers that are not aware of cross-reference streams. It is almost never
    ///     desirable to ignore them. The only time when you might want to use this feature is if you are testing creation of
    ///     hybrid PDF files and wish to see how a PDF consumer that doesn’t understand object and cross-reference streams
    ///     would interpret such a file.
    /// </summary>
    /// <see cref="Job" />
    public Job IgnoreXrefStreams()
    {
        _ignoreXrefStreams = string.Empty;
        return this;
    }
    #endregion

    #region Linearize
    /// <summary>
    ///     Create linearized (web-optimized) output files. Linearized files are formatted in a way that allows compliant
    ///     readers to begin displaying a PDF file before it is fully downloaded. Ordinarily, the entire file must be present
    ///     before it can be rendered because important cross-reference information typically appears at the end of the file.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Linearize()
    {
        _linearize = string.Empty;
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
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Encrypt(string userPassword, string ownerPassword, EncryptionOptions options)
    {
        _encryption = new Encryption(userPassword, ownerPassword, options);
        return this;
    }
    #endregion

    #region Decrypt
    /// <summary>
    ///     Create an output file with no encryption even if the input file is encrypted. This option overrides the default
    ///     behavior of preserving whatever encryption was present on the input file. This functionality is not intended to be
    ///     used for bypassing copyright restrictions or other restrictions placed on files by their producers. See also
    ///     --copy-encryption.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Decrypt()
    {
        _decrypt = string.Empty;
        return this;
    }
    #endregion

    #region CopyEncryption
    /// <summary>
    ///     Copy all encryption parameters, including the user password, the owner password, and all security restrictions,
    ///     from the specified file instead of preserving the encryption details from the input file. This works even if only
    ///     one of the user password or owner password is known. If the encryption file requires a password, use the
    ///     --encryption-file-password option to set it. Note that copying the encryption parameters from a file also copies
    ///     the first half of /ID from the file since this is part of the encryption parameters. This option can be useful if
    ///     you need to decrypt a file to make manual changes to it or to change it outside of qpdf, and then want to restore
    ///     the original encryption on the file without having to manual specify all the individual settings. See also
    ///     --decrypt.
    /// </summary>
    /// <param name="fileName">The file with full path</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    /// <exception cref="FileNotFoundException"></exception>
    public Job CopyEncryption(string fileName)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException(fileName);

        _copyEncryption = string.Empty;
        return this;
    }
    #endregion

    #region EncryptionFilePassword
    /// <summary>
    ///     f the file specified with <see cref="CopyEncryption" /> requires a password, supply the password using this option.
    ///     This option is necessary because the --password option applies to the input file, not the file from which
    ///     encryption is being copied.
    /// </summary>
    /// <param name="password"></param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job EncryptionFilePassword(string password)
    {
        _encryptionFilePassword = password;
        return this;
    }
    #endregion

    #region QPdf
    /// <summary>
    ///     Create a PDF file suitable for viewing and editing in a text editor. This is to edit the PDF code, not the page
    ///     contents. To edit a QDF file, your text editor must preserve binary data. In a QDF file, all streams that can be
    ///     uncompressed are uncompressed, and content streams are normalized, among other changes. The companion tool fix-qdf
    ///     can be used to repair hand-edited QDF files. QDF is a feature specific to the qpdf tool. For additional
    ///     information, see QDF Mode. Note that <see cref="Linearize" /> disables QDF mode.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job QPdf()
    {
        _qpdf = string.Empty;
        return this;
    }
    #endregion

    #region NoOriginalObjectIds
    /// <summary>
    ///     Suppresses inclusion of original object ID comments in QDF files. This can be useful when generating QDF files for
    ///     test purposes, particularly when comparing them to determine whether two PDF files have identical content. The
    ///     original object ID comment is there by default because it makes it easier to trace objects back to the original
    ///     file.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job NoOriginalObjectIds()
    {
        _noOriginalObjectIds = string.Empty;
        return this;
    }
    #endregion

    #region CompressStreams
    /// <summary>
    ///     By default, or with <see cref="CompressStreams" /> = <see cref="Enums.CompressStreams.Yes" />, qpdf will compress
    ///     streams using the flate compression algorithm (used by zip and gzip) unless those streams are compressed in some
    ///     other way. This analysis is made after qpdf attempts to uncompress streams and is therefore closely related to
    ///     <see cref="DecodeLevel" />. To suppress this behavior and leave streams streams uncompressed, use
    ///     <see cref="CompressStreams" /> = <see cref="Enums.CompressStreams.No" />. In QDF
    ///     mode (see QDF Mode and --qdf), the default is to leave streams uncompressed.
    /// </summary>
    /// <param name="compressStreams">
    ///     <see cref="CompressStreams" />
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job CompressStreams(CompressStreams compressStreams = Enums.CompressStreams.Yes)
    {
        _compressStreams = compressStreams;
        return this;
    }
    #endregion

    #region DecodeLevel
    /// <summary>
    ///     Controls which streams qpdf tries to decode. The default is <see cref="Enums.DecodeLevel.Generalized" />.
    /// </summary>
    /// <param name="decodeLevel">
    ///     <see cref="DecodeLevel" />
    /// </param>
    /// <returns></returns>
    public Job DecodeLevel(DecodeLevel decodeLevel = Enums.DecodeLevel.Generalized)
    {
        _decodeLevel = decodeLevel;
        return this;
    }
    #endregion

    // https://qpdf.readthedocs.io/en/stable/cli.html?highlight=ranges#option-stream-data

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
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job SplitPages(string n)
    {
        _splitPages = n;
        return this;
    }
    #endregion

    #region Run
    /// <summary>
    ///     Runs the job with the given parameters
    /// </summary>
    /// <returns>
    ///     <see cref="ExitCodes" />
    /// </returns>
    public ExitCodes Run()
    {
        var settings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore };
        var json = JsonConvert.SerializeObject(this, settings);
        return QPdfApi.Native.RunFromJSON(json);
    }
    #endregion
}