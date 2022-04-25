//
// Job.cs
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
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QPdfNet.Enums;
using QPdfNet.Interfaces;
using QPdfNet.Interop;
using QPdfNet.Loggers;

// ReSharper disable UnusedMember.Global

namespace QPdfNet;

/// <summary>
///     Represents a qpdf job
/// </summary>
/// <remarks>
///     https://qpdf.readthedocs.io/en/stable/qpdf-job.html
/// </remarks>
public class Job
{
    #region Fields
    [JsonProperty("inputFile")] private string? _inputFile;
    [JsonProperty("outputFile")] private string? _outputFile;
    [JsonProperty("empty")] private string? _empty;
    [JsonProperty("replaceInput")] private string? _replaceInput;
    [JsonProperty("warningExit0")] private string? _warningExit0;
    [JsonProperty("password")] private string? _password;
    [JsonProperty("passwordFile")] private string? _passwordFile;
    [JsonProperty("verbose")] private string? _verbose;
    [JsonProperty("noWarn")] private string? _noWarn;
    [JsonProperty("deterministicId")] private string? _deterministicId;
    [JsonProperty("allowWeakCrypto")] private string? _allowWeakCrypto;
    [JsonProperty("keepFilesOpen")] private string? _keepFilesOpen;
    [JsonProperty("keepFilesOpenThreshold")] private string? _keepFilesOpenThreshold;
    [JsonProperty("passwordIsHexKey")] private string? _passwordIsHexKey;
    [JsonProperty("suppressPasswordRecovery")] private string? _suppressPasswordRecovery;
    [JsonProperty("passwordMode")] private PasswordMode _passwordMode;
    [JsonProperty("suppressRecovery")] private string? _suppressRecovery;
    [JsonProperty("ignoreXrefStreams")] private string? _ignoreXrefStreams;
    [JsonProperty("linearize")] private string? _linearize;
    [JsonProperty("encrypt")] private Encryption? _encryption;
    [JsonProperty("decrypt")] private string? _decrypt;
    [JsonProperty("copyEncryption")] private string? _copyEncryption;
    [JsonProperty("encryptionFilePassword")] private string? _encryptionFilePassword;
    [JsonProperty("qdf")] private string? _qdf;
    [JsonProperty("noOriginalObjectIds")] private string? _noOriginalObjectIds;
    [JsonProperty("compressStreams")] private string? _compressStreams;
    [JsonProperty("decodeLevel")] private DecodeLevel _decodeLevel;
    [JsonProperty("streamData")] private StreamData _streamData;
    [JsonProperty("recompressFlate")] private string? _recompressFlate;
    [JsonProperty("compressionLevel")] private string? _compressionLevel;
    [JsonProperty("normalizeContent")] private string? _normalizeContent;
    [JsonProperty("objectStreams")] private ObjectStreams _objectStreams;
    [JsonProperty("preserveUnreferenced")] private string? _preserveUnreferenced;
    [JsonProperty("removeUnreferencedResources")] private AutoYesNo _removeUnreferencedResources;
    [JsonProperty("preserveUnreferencedResources")] private string? _preserveUnreferencedResources;
    [JsonProperty("newlineBeforeEndstream")] private string? _newlineBeforeEndstream;
    [JsonProperty("coalesceContents")] private string? _coalesceContents;
    [JsonProperty("externalizeInlineImages")] private string? _externalizeInlineImages;
    [JsonProperty("iiMinBytes")] private string? _iiMinBytes;
    [JsonProperty("minVersion")] private string? _minVersion;
    [JsonProperty("forceVersion")] private string? _forceVersion;
    [JsonProperty("collate")] private string? _collate;
    [JsonProperty("pages")] private List<Pages>? _pages;
    [JsonProperty("splitPages")] private string? _splitPages;
    [JsonProperty("overlay")] private Dictionary<string, string>? _overlay;
    [JsonProperty("underlay")] private Dictionary<string, string>? _underlay;
    [JsonProperty("addAttachment")] private List<AddAttachment>? _addAttachment;
    [JsonProperty("copyAttachments")] private List<CopyAttachment>? _copyAttachments;
    [JsonProperty("removeAttachment")] private List<string>? _removeAttachment;
    [JsonProperty("flattenRotation")] private string? _flattenRotation;
    [JsonProperty("flattenAnnotations")] private FlattenAnnotations _flattenAnnotation;
    [JsonProperty("rotate")] private string? _rotate;
    [JsonProperty("generateAppearances")] private string? _generateAppearances;
    [JsonProperty("optimizeImages")] private string? _optimizeImages;
    [JsonProperty("oiMinWidth")] private string? _oiMinWidth;
    [JsonProperty("oiMinHeight")] private string? _oiMinHeight;
    [JsonProperty("oiMinArea")] private string? _oiMinArea;
    [JsonProperty("keepInlineImages")] private string? _keepInlineImages;
    [JsonProperty("removePageLabels")] private string? _removePageLabels;
    [JsonProperty("isEncrypted")] private string? _isEncrypted;
    [JsonProperty("requiresPassword")] private string? _requiresPassword;
    [JsonProperty("check")] private string? _check;
    [JsonProperty("showEncryption")] private string? _showEncryption;
    [JsonProperty("showEncryptionKey")] private string? _showEncryptionKey;
    [JsonProperty("checkLinearization")] private string? _checkLinearization;
    [JsonProperty("showLinearization")] private string? _showLinearization;
    [JsonProperty("showXref")] private string? _showXref;
    [JsonProperty("showObject")] private string? _showObject;
    [JsonProperty("rawStreamData")] private string? _rawStreamData;
    [JsonProperty("filteredStreamData")] private string? _filteredStreamData;
    [JsonProperty("showNpages")] private string? _showNpages;
    [JsonProperty("showPages")] private string? _showPages;
    [JsonProperty("withImages")] private string? _withImages;
    [JsonProperty("listAttachments")] private string? _listAttachments;
    [JsonProperty("showAttachment")] private string? _showAttachment;
    [JsonProperty("json")] private string? _json;
    [JsonProperty("jsonKey")] private List<string>? _jsonKey;
    [JsonProperty("jsonObject")] private List<string>? _jsonObject;
    [JsonProperty("staticId")] private string? _staticId;
    [JsonProperty("staticAesIv")] private string? _staticAesIv;
    [JsonProperty("linearizePass1")] private string? _linearizePass1;
    #endregion

    #region Constructor
    /// <summary>
    ///     Makes this object
    /// </summary>
    /// <param name="logger">When set then logging is written to this interface</param>
    public Job(ILogger? logger = null)
    {
        if (logger != null)
            Logger.LoggerInterface = logger;
    }
    #endregion

    #region InputFile
    /// <summary>
    ///     The input PDF file
    /// </summary>
    /// <param name="fileName">The input file</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job InputFile(string fileName)
    {
        Logger.LogInformation($"Opening PDF file '{fileName}'");
        _inputFile = fileName;
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
        Logger.LogInformation($"Writing output to file '{fileName}'");
        _outputFile = fileName;
        return this;
    }
    #endregion

    #region Empty
    /// <summary>
    ///     This option may be given in place of infile. This causes qpdf to use a dummy input file that contains zero pages.
    ///     This option is useful in conjunction with <see cref="Pages"/>. See Page Selection for details.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Empty()
    {
        Logger.LogInformation("Creating empty PDF");
        _empty = string.Empty;
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
        Logger.LogInformation(string.IsNullOrEmpty(_inputFile)
            ? $"Replacing input file '{_inputFile}'"
            : "Replacing input file");

        _replaceInput = string.Empty;
        return this;
    }
    #endregion

    #region WarningExit0
    /// <summary>
    ///     If there were warnings only and no errors, exit with exit code 0 instead of 3. When combined with
    ///     <see cref="NoWarn" />, the effect is for qpdf to completely ignore warnings.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job WarningExit0()
    {
        Logger.LogInformation("If there were warnings only and no errors, exit with exit code 0 instead of 3");
        _warningExit0 = string.Empty;
        return this;
    }
    #endregion

    #region Password
    /// <summary>
    ///     Specifies a password for accessing encrypted, password-protected files. To read the password from a file or
    ///     standard input
    /// </summary>
    /// <param name="password">The password to open the <see cref="InputFile"/></param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Password(string password)
    {
        Logger.LogInformation($"Setting password to '{new string('*', password.Length)}'");
        _password = password;
        return this;
    }
    #endregion

    #region PasswordFile
    /// <summary>
    ///     Reads the first line from the specified <paramref name="file"/> and uses it as the password for accessing encrypted files
    /// </summary>
    /// <param name="file">The file with full path</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    /// <exception cref="FileNotFoundException"></exception>
    public Job PasswordFile(string file)
    {
        Logger.LogInformation($"Setting password file to '{file}'");

        if (!System.IO.File.Exists(file))
            throw new FileNotFoundException(file);

        _passwordFile = string.Empty;
        return this;
    }
    #endregion

    #region Verbose
    /// <summary>
    ///     Increase verbosity of output. This includes information about files created, image optimization, and several other operations.
    ///     In some cases, it also displays additional information when inspection options (see PDF Inspection) are used.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Verbose()
    {
        Logger.LogInformation("Increase verbosity of output");
        _verbose = string.Empty;
        return this;
    }
    #endregion

    #region NoWarn
    /// <summary>
    ///     Suppress writing of warnings to stderr. If warnings were detected and suppressed, qpdf will still exit with exit
    ///     code 3. To completely ignore warnings, also specify <see cref="WarningExit0" />. Use with caution as qpdf is not
    ///     always successful in recovering from situations that cause warnings to be issued.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job NoWarn()
    {
        Logger.LogInformation("Suppress writing of warnings to stderr");
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
    ///     See also <see cref="StaticId" />.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job DeterministicId()
    {
        Logger.LogInformation("Generate a secure, random document ID using deterministic values");
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
        Logger.LogInformation("Allowing weak crypto");
        _allowWeakCrypto = string.Empty;
        return this;
    }
    #endregion

    #region KeepFilesOpen
    /// <summary>
    ///     This option controls whether qpdf keeps individual files open while merging. By default, qpdf keeps files open when
    ///     merging unless more than 200 files are specified, in which case files are opened as needed and closed when finished.
    ///     Repeatedly opening and closing files may impose a large performance penalty with some file systems, especially networked
    ///     file systems. If you know that you have a large enough open file limit and are suffering from performance problems, or
    ///     if you have an open file limit smaller than 200, you can use this option to override the default behavior by specifying
    ///     <see cref="KeepFilesOpen"/> = <c>true</c> to force qpdf to keep files open or <see cref="KeepFilesOpen"/> = <c>false</c>
    ///     to force it to only open files as needed. See also <see cref="KeepFilesOpenThreshold"/>.
    /// </summary>
    /// <param name="keepOpen"><c>true</c> or <c>false</c></param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job KeepFilesOpen(bool keepOpen)
    {
        if (keepOpen)
            Logger.LogInformation("Keeping files open");

        _keepFilesOpen = keepOpen ? "y" : "n";
        return this;
    }
    #endregion

    #region KeepFilesOpenThreshold
    /// <summary>
    ///     If specified, overrides the default value of <c>200</c> used as the threshold for qpdf deciding whether or not to keep
    ///     files open. See <see cref="KeepFilesOpen"/> for details.
    /// </summary>
    /// <param name="count">The amount of files to keep open</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job KeepFilesOpenThreshold(int count = 200)
    {
        if (count < 1)
            throw new ArgumentOutOfRangeException(nameof(count), "Needs to be a value of 1 or greater");

        Logger.LogInformation($"Keeping files open threshold set to {count}");

        _keepFilesOpenThreshold = count.ToString();
        return this;
    }
    #endregion

    #region PasswordIsHexKey
    /// <summary>
    ///     Overrides the usual computation/retrieval of the PDF file’s encryption key from user/owner password with an
    ///     explicit specification of the encryption key. When this option is specified, the parameter to the
    ///     <see cref="Password" /> option is interpreted as a hexadecimal-encoded key value. This only applies to the password
    ///     used to open the main input file. It does not apply to other files opened by --pages or other options or to files
    ///     being written. Most users will never have a need for this option, and no standard viewers support this mode of
    ///     operation, but it can be useful for forensic or investigatory purposes. For example, if a PDF file is encrypted with
    ///     an unknown password, a brute-force attack using the key directly is sometimes more efficient than one using the
    ///     password.Also, if a file is heavily damaged, it may be possible to derive the encryption key and recover parts of
    ///     the file using it directly.To expose the encryption key used by an encrypted file that you can open normally, use
    ///     the <see cref="ShowEncryptionKey" /> option.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job PasswordIsHexKey()
    {
        Logger.LogInformation("Interpreting password as a hexadecimal-encoded key value");
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
        Logger.LogInformation("Suppressing password recovery");
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
    /// <param name="mode">
    ///     <see cref="PasswordMode" />
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job PasswordMode(PasswordMode mode)
    {
        Logger.LogInformation($"Setting password mode to '{mode}'");
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
        Logger.LogInformation("Preventing attempt to reconstruct a file’s cross reference table when there are errors reading objects from the file");
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
        Logger.LogInformation("Ignoring any cross-reference streams");
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
        Logger.LogInformation("Create linearized (web-optimized) output files");
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
    public Job Encrypt(string? userPassword, string ownerPassword, IEncryption options)
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
    ///     <see cref="CopyEncryption"/>.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Decrypt()
    {
        Logger.LogInformation("Creating PDF output file with no encryption");
        _decrypt = string.Empty;
        return this;
    }
    #endregion

    #region CopyEncryption
    /// <summary>
    ///     Copy all encryption parameters, including the user password, the owner password, and all security restrictions,
    ///     from the specified file instead of preserving the encryption details from the input file. This works even if only
    ///     one of the user password or owner password is known. If the encryption file requires a password, use the
    ///     <see cref="EncryptionFilePassword" /> option to set it. Note that copying the encryption parameters from a file
    ///     also copies the first half of /ID from the file since this is part of the encryption parameters. This option can
    ///     be useful if you need to decrypt a file to make manual changes to it or to change it outside of qpdf, and then want
    ///     to restore the original encryption on the file without having to manual specify all the individual settings. See also
    ///     <see cref="Decrypt"/>.
    /// </summary>
    /// <param name="file">The file with full path</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    /// <exception cref="FileNotFoundException"></exception>
    public Job CopyEncryption(string file)
    {
        if (!System.IO.File.Exists(file))
        {
            Logger.LogError($"The file '{file}' could not be found");
            throw new FileNotFoundException(file);
        }

        Logger.LogInformation($"Copying encryption from file '{file}'");

        _copyEncryption = string.Empty;
        return this;
    }
    #endregion

    #region EncryptionFilePassword
    /// <summary>
    ///     If the file specified with <see cref="CopyEncryption" /> requires a password, supply the password using this option.
    ///     This option is necessary because the <see cref="Password"/> option applies to the <see cref="InputFile"/>, not the
    ///     file from which encryption is being copied.
    /// </summary>
    /// <param name="password">The password</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job EncryptionFilePassword(string password)
    {
        Logger.LogInformation($"Setting encryption file password to '{new string('*', password.Length)}'");
        _encryptionFilePassword = password;
        return this;
    }
    #endregion

    #region Qdf
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
    public Job Qdf()
    {
        Logger.LogInformation("Creating a PDF file suitable for viewing and editing in a text editor");
        _qdf = string.Empty;
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
        Logger.LogInformation("Suppresses inclusion of original object ID comments in QDF files");
        _noOriginalObjectIds = string.Empty;
        return this;
    }
    #endregion

    #region CompressStreams
    /// <summary>
    ///     By default, or with use <paramref name="compress" /> = true, qpdf will compress streams using the flate compression
    ///     algorithm (used by zip and gzip) unless those streams are compressed in some other way. This analysis is
    ///     made after qpdf attempts to uncompress streams and is therefore closely related to <see cref="DecodeLevel" />.
    ///     To suppress this behavior and leave streams streams uncompressed, use <paramref name="compress" /> = <c>false</c>.
    ///     In QDF mode (see QDF Mode and <see cref="Qdf" />), the default is to leave streams uncompressed.
    /// </summary>
    /// <param name="compress"><c>true</c> or <c>false</c></param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job CompressStreams(bool compress)
    {
        Logger.LogInformation(compress ? "Compressing streams" : "Disabling stream compression");

        _compressStreams = compress ? "y" : "n";
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
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job DecodeLevel(DecodeLevel decodeLevel = Enums.DecodeLevel.Generalized)
    {
        Logger.LogInformation($"Setting stream decode level to '{decodeLevel}'");
        _decodeLevel = decodeLevel;
        return this;
    }
    #endregion

    #region StreamData
    /// <summary>
    ///     Controls transformation of stream data. This option predates the <see cref="CompressStreams" /> and
    ///     <see cref="DecodeLevel" /> options. Those options can be used to achieve the same effect with more control
    /// </summary>
    /// <param name="streamData"><see cref="Enums.StreamData"/></param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job StreamData(StreamData streamData = Enums.StreamData.Compress)
    {
        Logger.LogInformation($"Setting stream data to '{streamData}'");
        _streamData = streamData;
        return this;
    }
    #endregion

    #region RecompressFlate
    /// <summary>
    ///     The default generalized compression scheme used by PDF is flate (/FlateDecode), which is the same as used by zip
    ///     and gzip. Usually qpdf just leaves these alone. This option tells qpdf to uncompress and recompress streams
    ///     compressed with flate. This can be useful when combined with <see cref="CompressionLevel" />. Using this option may
    ///     make qpdf much slower when writing output files.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job RecompressFlate()
    {
        Logger.LogInformation("Re compressing streams that are compressed with flate");
        _recompressFlate = string.Empty;
        return this;
    }
    #endregion

    #region CompressionLevel
    /// <summary>
    ///     When writing new streams that are compressed with /FlateDecode, use the specified compression
    ///     <paramref name="level" />. The value of <paramref name="level" /> should be a number from 1 to 9 and is passed
    ///     directly to zlib, which implements deflate compression. Lower numbers compress less and are faster; higher
    ///     numbers compress more and are slower. Note that qpdf doesn’t uncompress and recompress streams compressed with flate
    ///     by default. To have this option apply to already compressed streams, you should also specify <see cref="RecompressFlate" />.
    ///     If your goal is to shrink the size of PDF files, you should also use <see cref="ObjectStreams" />. If you omit this option,
    ///     qpdf defers to the compression library’s default behavior.
    /// </summary>
    /// <param name="level">A value in the range 1 to 9</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job CompressionLevel(int level)
    {
        if (level is < 1 or > 9)
        {
            Logger.LogError($"Compression level '{level}' should be in the range 1 to 9");
            throw new ArgumentOutOfRangeException(nameof(level), "Should be in the range 1 to 9");
        }

        Logger.LogInformation($"Setting compression level to {level}");
        _compressionLevel = level.ToString();
        return this;
    }
    #endregion

    #region NormalizeContent
    /// <summary>
    ///     Enables or disables normalization of newlines in PDF content streams to UNIX-style newlines, which is useful for
    ///     viewing files in a programmer-friendly text edit across multiple platforms. Content normalization is off by default,
    ///     but is automatically enabled by <see cref="Qdf" /> (see also QDF Mode). It is not recommended to use this
    ///     option for production use. If qpdf runs into any lexical errors while normalizing content, it will print a warning
    ///     indicating that content may be damaged.
    /// </summary>
    /// <param name="normalize"><c>true</c> or <c>false</c></param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job NormalizeContent(bool normalize)
    {
        if (normalize)
            Logger.LogInformation("Normalizing newlines in PDF content stream to UNIX-style newlines");

        _normalizeContent = normalize ? "y" : "n";
        return this;
    }
    #endregion

    #region ObjectStreams
    /// <summary>
    ///     Controls handling of object streams. Object streams are PDF streams that contain other objects. Putting objects
    ///     into object streams allows the PDF objects themselves to be compressed, which can result in much smaller PDF files.
    ///     Combining this option with <see cref="CompressionLevel" /> and <see cref="RecompressFlate" /> can often result in
    ///     the creation of smaller PDF files.
    /// </summary>
    /// <param name="objectStreams">
    ///     <see cref="Enums.ObjectStreams" />
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ObjectStreams(ObjectStreams objectStreams)
    {
        Logger.LogInformation($"Setting object stream handling to '{objectStreams}'");
        _objectStreams = objectStreams;
        return this;
    }
    #endregion

    #region PreserveUnreferenced
    /// <summary>
    ///     Tells qpdf to preserve objects that are not referenced when writing the file. Ordinarily any object that is not
    ///     referenced in a traversal of the document from the trailer dictionary will be discarded. Disabling this default
    ///     behavior may be useful in working with some damaged files or inspecting files with known unreferenced objects.
    ///     This flag is ignored for linearized files and has the effect of causing objects in the new file to be written
    ///     ordered by object ID from the original file. This does not mean that object numbers will be the same since qpdf may
    ///     create stream lengths as direct or indirect differently from the original file, and the original file may have gaps
    ///     in its numbering. See also <see cref="PreserveUnreferencedResources" />, which does something completely different.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job PreserveUnreferenced()
    {
        Logger.LogInformation("Preserving objects that are not referenced when writing the file");
        _preserveUnreferenced = string.Empty;
        return this;
    }
    #endregion

    #region RemoveUnreferencedResources
    /// <summary>
    ///     Starting with qpdf 8.1, when splitting pages, qpdf is able to attempt to remove images and fonts that are not used
    ///     by a page even if they are referenced in the page’s resources dictionary. When shared resources are in use, this
    ///     behavior can greatly reduce the file sizes of split pages, but the analysis is very slow. In versions from 8.1
    ///     through 9.1.1, qpdf did this analysis by default. Starting in qpdf 10.0.0, if auto is used, qpdf does a quick
    ///     analysis of the file to determine whether the file is likely to have unreferenced objects on pages, a pattern that
    ///     frequently occurs when resource dictionaries are shared across multiple pages and rarely occurs otherwise. If it
    ///     discovers this pattern, then it will attempt to remove unreferenced resources. Usually this means you get the
    ///     slower splitting speed only when it’s actually going to create smaller files. You can suppress removal of
    ///     unreferenced resources altogether by specifying no or force qpdf to do the full algorithm by specifying
    ///     <see cref="AutoYesNo.Yes" />.
    /// </summary>
    /// <param name="removeUnreferencedResources">
    ///     <see cref="AutoYesNo" />
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job RemoveUnreferencedResources(AutoYesNo removeUnreferencedResources = AutoYesNo.Auto)
    {
        _removeUnreferencedResources = removeUnreferencedResources;
        return this;
    }
    #endregion

    #region PreserveUnreferencedResources
    /// <summary>
    ///     This is a synonym for <see cref="RemoveUnreferencedResources" /> = <see cref="AutoYesNo.No" />. See
    ///     <see cref="RemoveUnreferencedResources" />. See also <see cref="PreserveUnreferenced" />, which does
    ///     something completely different. To reduce confusion, you should use <see cref="RemoveUnreferencedResources" />
    ///     = <see cref="AutoYesNo.No" /> instead.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job PreserveUnreferencedResources()
    {
        _preserveUnreferencedResources = string.Empty;
        return this;
    }
    #endregion

    #region NewlineBeforeEndstream
    /// <summary>
    ///     Tell qpdf to insert a newline before the endstream keyword, not counted in the length, after any stream content
    ///     even if the last character of the stream was a newline. This may result in two newlines in some cases. This is a
    ///     requirement of PDF/A. While qpdf doesn’t specifically know how to generate PDF/A-compliant PDFs, this at least
    ///     prevents it from removing compliance on already compliant files.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job NewlineBeforeEndstream()
    {
        _newlineBeforeEndstream = string.Empty;
        return this;
    }
    #endregion

    #region CoalesceContents
    /// <summary>
    ///     When a page’s contents are split across multiple streams, this option causes qpdf to combine them into a single
    ///     stream. Use of this option is never necessary for ordinary usage, but it can help when working with some files in
    ///     some cases. For example, this can be combined with QDF mode or content normalization to make it easier to look at
    ///     all of a page’s contents at once. It is common for PDF writers to create multiple content streams for a variety of
    ///     reasons such as making it easier to modify page contents and splitting very large content streams so PDF viewers
    ///     may be able to use less memory.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job CoalesceContents()
    {
        _coalesceContents = string.Empty;
        return this;
    }
    #endregion

    #region ExternalizeInlineImages
    /// <summary>
    ///     Convert inline images to regular images. By default, images whose data is at least 1,024 bytes are converted when
    ///     this option is selected. Use <see cref="IiMinBytes" /> to change the size threshold. This option is implicitly
    ///     selected when <see cref="OptimizeImages" /> is selected unless <see cref="KeepInlineImages" /> is also specified.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ExternalizeInlineImages()
    {
        _externalizeInlineImages = string.Empty;
        return this;
    }
    #endregion

    #region IiMinBytes
    /// <summary>
    ///     Avoid converting inline images whose size is below the specified minimum size to regular images. The default is
    ///     <b>1024</b> bytes. Use <b>0</b> for no minimum.
    /// </summary>
    /// <param name="size">The minimum size</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job IiMinBytes(int size = 1024)
    {
        if (size < 0)
            throw new ArgumentOutOfRangeException(nameof(size));

        _iiMinBytes = size.ToString();
        return this;
    }
    #endregion

    #region MinVersion
    /// <summary>
    ///     Force the PDF version of the output file to be at least version. In other words, if the input file has a lower
    ///     version than the specified version, the specified version will be used. If the input file has a higher version, the
    ///     input file’s original version will be used. It is seldom necessary to use this option since qpdf will automatically
    ///     increase the version as needed when adding features that require newer PDF readers. The version number may be expressed
    ///     in the form major.minor[.extension-level]. If .extension-level, is given, version is interpreted as major.minor at
    ///     extension level extension-level. For example, version 1.7.8 represents version 1.7 at extension level 8. Note that
    ///     minimal syntax checking is done on the command line. qpdf does not check whether the specified version is actually required.
    /// </summary>
    /// <param name="version">The minimum version</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job MinVersion(string version)
    {
        if (string.IsNullOrWhiteSpace(version))
            throw new ArgumentOutOfRangeException(nameof(version));

        _minVersion = version;
        return this;
    }
    #endregion

    #region ForceVersion
    /// <summary>
    ///     This option forces the PDF version to be the exact version specified even when the file may have content that is
    ///     not supported in that version. The version number is interpreted in the same way as with <see cref="MinVersion"/>
    ///     so that extension levels can be set. In some cases, forcing the output file’s PDF version to be lower than that of the
    ///     input file will cause qpdf to disable certain features of the document. Specifically, 256-bit keys are disabled if
    ///     the version is less than 1.7 with extension level 8 (except the deprecated, unsupported “R5” format is allowed with
    ///     extension levels 3 through 7), AES encryption is disabled if the version is less than 1.6, clear text metadata and
    ///     object streams are disabled if less than 1.5, 128-bit encryption keys are disabled if less than 1.4, and all
    ///     encryption is disabled if less than 1.3. Even with these precautions, qpdf won’t be able to do things like
    ///     eliminate use of newer image compression schemes, transparency groups, or other features that may have been added
    ///     in more recent versions of PDF. As a general rule, with the exception of big structural things like the use of object
    ///     streams or AES encryption, PDF viewers are supposed to ignore features they don’t support. This means that forcing the
    ///     version to a lower version may make it possible to open your PDF file with an older version, though bear in mind that
    ///     some of the original document’s functionality may be lost.
    /// </summary>
    /// <param name="version">The forced version</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ForceVersion(string version)
    {
        if (string.IsNullOrWhiteSpace(version))
            throw new ArgumentOutOfRangeException(nameof(version));

        _forceVersion = version;
        return this;
    }
    #endregion

    #region Pages
    /// <summary>
    ///     This method starts page selection options, which are used to select pages from one or more input files to perform
    ///     operations such as splitting, merging, and collating files.
    /// </summary>
    /// <remarks>
    ///     See https://qpdf.readthedocs.io/en/stable/cli.html#page-selection about how this method works
    /// </remarks>
    /// <param name="file">The file</param>
    /// <param name="range">The page range</param>
    /// <param name="password">The password or <c>null</c> when not needed</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Pages(string file, string range, string? password = null)
    {
        _pages ??= new List<Pages>();
        _pages.Add(new Pages(file, range, password));
        return this;
    }
    #endregion

    #region Collate
    /// <summary>
    ///     This option causes qpdf to collate rather than concatenate pages specified with <see cref="Pages"/>. With a numeric parameter,
    ///     collate in groups of <paramref name="n" />. The default is <b>1</b>.
    /// </summary>
    /// <param name="n">
    ///     A value greater than zero
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Collate(int n = 1)
    {
        if (n <= 0)
            throw new ArgumentOutOfRangeException(nameof(n), "Value should be greater than zero");

        _collate = n.ToString();
        return this;
    }
    #endregion

    #region SplitPages
    /// <summary>
    ///     Write each group of <paramref name="n" /> pages to a separate  <see cref="OutputFile" />. If <paramref name="n" />
    ///     is not specified, create single pages. Output file names are generated as follows:<br/>
    ///     If the string %d appears in the <see cref="OutputFile" /> name, it is replaced with a range of zero-padded page
    ///     numbers starting from 1. Otherwise, if the output file name ends in .pdf(case insensitive), a zero-padded page
    ///     range, preceded by a dash, is inserted before the file extension. Otherwise, the file name is appended with a
    ///     zero-padded page range preceded by a dash.Zero padding is added to all page numbers in file names so that all
    ///     the numbers are the same length, which causes the output filenames to sort lexically in numerical order. Page
    ///     ranges are a single number in the case of single-page groups or two numbers separated by a dash otherwise. for
    ///     testing
    /// </summary>
    /// <param name="n">
    ///     A value greater than zero
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job SplitPages(int n = 1)
    {
        if (n <= 0)
            throw new ArgumentOutOfRangeException(nameof(n), "Value should be greater than zero");

        _splitPages = n.ToString();
        return this;
    }
    #endregion

    #region Overlay
    /// <summary>
    ///     <see cref="Overlay"/> options are processed late, so they can be combined with other options like merging and will
    ///     apply to the final output. The <see cref="Underlay"/> options work the same way, except <see cref="Underlay"/> pages
    ///     are drawn underneath the page to which they are applied, possibly obscured by the original page, and
    ///     <see cref="Overlay"/> files are drawn on top of the page to which they are applied, possibly obscuring the page. You
    ///     can combine both, but you can only specify each option at most one time. The default behavior of <see cref="Overlay"/>
    ///     and <see cref="Underlay"/> is that pages are taken from the <see cref="Overlay"/>/<see cref="Underlay"/> file in
    ///     sequence and applied to corresponding pages in the output until there are no more output pages.If the
    ///     <see cref="Overlay"/> or <see cref="Underlay"/> file runs out of pages, remaining output pages are left alone
    /// </summary>
    /// <param name="file">The file with it's full path</param>
    /// <param name="to">
    ///     Specify a page range (see Page Ranges) that indicates which pages in the output should have the
    ///     <see cref="Overlay"/>/<see cref="Underlay"/> applied. If not specified, <see cref="Overlay"/>/<see cref="Underlay"/>
    ///     are applied to all pages.
    /// </param>
    /// <param name="from">
    ///     Specify a page range that indicates which pages in the <see cref="Overlay"/>/<see cref="Underlay"/> file will be used
    ///     for <see cref="Overlay"/> or <see cref="Underlay"/>. If not specified, all pages will be used. The
    ///     <paramref name="from"/> pages are used until they are exhausted, after which any pages specified with
    ///     <paramref name="repeat" /> are used. If you are using the <paramref name="repeat" /> option, you can use
    ///     <paramref name="from"/> to provide an empty set of <paramref name="from"/> pages.
    /// </param>
    /// <param name="repeat">
    ///     Specify an optional page range that indicates which pages in the <see cref="Overlay"/>/<see cref="Underlay"/> file
    ///     will be repeated after the <paramref name="from"/> pages are used up. If you want to apply a repeat a range of
    ///     pages starting with the first page of output, you can explicitly use <paramref name="from"/>.
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Overlay(string file, string? to = null, string? from = null, string? repeat = null)
    {
        if (!System.IO.File.Exists(file))
            throw new FileNotFoundException($"The file '{file}' could not be found");

        _overlay = new Dictionary<string, string> { { "file", file } };

        if (!string.IsNullOrWhiteSpace(to))
            _overlay.Add("to", to);

        if (!string.IsNullOrWhiteSpace(from))
            _overlay.Add("from", from);

        if (!string.IsNullOrWhiteSpace(repeat))
            _overlay.Add("repeat", repeat);

        return this;
    }
    #endregion

    #region Underlay
    /// <summary>
    ///     <see cref="Overlay"/> options are processed late, so they can be combined with other options like merging and will
    ///     apply to the final output. The <see cref="Underlay"/> options work the same way, except <see cref="Underlay"/> pages
    ///     are drawn underneath the page to which they are applied, possibly obscured by the original page, and
    ///     <see cref="Overlay"/> files are drawn on top of the page to which they are applied, possibly obscuring the page. You
    ///     can combine both, but you can only specify each option at most one time. The default behavior of <see cref="Overlay"/>
    ///     and <see cref="Underlay"/> is that pages are taken from the <see cref="Overlay"/>/<see cref="Underlay"/> file in
    ///     sequence and applied to corresponding pages in the output until there are no more output pages.If the
    ///     <see cref="Overlay"/> or <see cref="Underlay"/> file runs out of pages, remaining output pages are left alone
    /// </summary>
    /// <param name="file">The file with it's full path</param>
    /// <param name="to">
    ///     Specify a page range (see Page Ranges) that indicates which pages in the output should have the
    ///     <see cref="Overlay"/>/<see cref="Underlay"/> applied. If not specified, <see cref="Overlay"/>/<see cref="Underlay"/>
    ///     are applied to all pages.
    /// </param>
    /// <param name="from">
    ///     Specify a page range that indicates which pages in the <see cref="Overlay"/>/<see cref="Underlay"/> file will be used
    ///     for <see cref="Overlay"/> or <see cref="Underlay"/>. If not specified, all pages will be used. The
    ///     <paramref name="from"/> pages are used until they are exhausted, after which any pages specified with
    ///     <paramref name="repeat" /> are used. If you are using the <paramref name="repeat" /> option, you can use
    ///     <paramref name="from"/> to provide an empty set of <paramref name="from"/> pages.
    /// </param>
    /// <param name="repeat">
    ///     Specify an optional page range that indicates which pages in the <see cref="Overlay"/>/<see cref="Underlay"/> file
    ///     will be repeated after the <paramref name="from"/> pages are used up. If you want to apply a repeat a range of
    ///     pages starting with the first page of output, you can explicitly use <paramref name="from"/>.
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Underlay(string file, string? to = null, string? from = null, string? repeat = null)
    {
        if (!System.IO.File.Exists(file))
            throw new FileNotFoundException($"The file '{file}' could not be found");

        _underlay = new Dictionary<string, string> { { "file", file } };

        if (!string.IsNullOrWhiteSpace(to))
            _underlay.Add("to", to);

        if (!string.IsNullOrWhiteSpace(from))
            _underlay.Add("from", from);

        if (!string.IsNullOrWhiteSpace(repeat))
            _underlay.Add("repeat", repeat);

        return this;
    }
    #endregion

    #region AddAttachment
    /// <summary>
    ///     This flag starts add attachment options, which are used to add attachments to a file.
    ///     The  flag and its options may be repeated to add multiple attachments
    /// </summary>
    /// <param name="fileName">
    ///     Specify the filename to be used for the attachment. This is what is usually displayed to the
    ///     user and is the name most graphical PDF viewers will use when saving a file. It defaults to the last element of the
    ///     attached file’s filename.
    /// </param>
    /// <param name="description">Supply descriptive text for the attachment, displayed by some PDF viewers.</param>
    /// <param name="replace">
    ///     Indicate that any existing attachment with the same key should be replaced by the new attachment.
    ///     Otherwise, qpdf gives an error if an attachment with that key is already present.
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    /// <exception cref="FileNotFoundException"></exception>
    public Job AddAttachment(string fileName, string? description = null, bool replace = false)
    {
        var addAttachment = new AddAttachment(fileName, description, replace);
        _addAttachment ??= new List<AddAttachment>();
        _addAttachment.Add(addAttachment);
        return this;
    }

    /// <summary>
    ///     This flag starts add attachment options, which are used to add attachments to a file.
    ///     The  flag and its options may be repeated to add multiple attachments
    /// </summary>
    /// <param name="file">The file with it's full path</param>
    /// <param name="key">
    ///     Specify the key to use for the attachment in the embedded files table. It defaults to the last
    ///     element of the attached file’s filename.
    /// </param>
    /// <param name="fileName">
    ///     Specify the filename to be used for the attachment. This is what is usually displayed to the
    ///     user and is the name most graphical PDF viewers will use when saving a file. It defaults to the last element of the
    ///     attached file’s filename.
    /// </param>
    /// <param name="creationDate">Specify the attachment’s creation date in PDF format; defaults to the current time</param>
    /// <param name="modData">Specify the attachment’s modification date in PDF format; defaults to the current time.</param>
    /// <param name="mimeType">
    ///     Specify the mime type for the attachment, such as text/plain, application/pdf, image/png, etc.
    ///     The qpdf library does not automatically determine the mime type.
    /// </param>
    /// <param name="description">Supply descriptive text for the attachment, displayed by some PDF viewers.</param>
    /// <param name="replace">
    ///     Indicate that any existing attachment with the same key should be replaced by the new attachment.
    ///     Otherwise, qpdf gives an error if an attachment with that key is already present.
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    /// <exception cref="FileNotFoundException"></exception>
    public Job AddAttachment(
        string file,
        string key,
        string fileName,
        DateTime creationDate,
        DateTime modData,
        string mimeType,
        string? description = null,
        bool replace = false)
    {
        var addAttachment = new AddAttachment(file, key, fileName, creationDate, modData, mimeType, description: description, replace: replace);
        _addAttachment ??= new List<AddAttachment>();
        _addAttachment.Add(addAttachment);
        return this;
    }
    #endregion

    #region CopyAttachmentsFrom
    /// <summary>
    ///     This flag starts add attachment options, which are used to add attachments to a file.
    ///     The  flag and its options may be repeated to add multiple attachments
    /// </summary>
    /// <param name="file">The file to copy from with its full path</param>
    /// <param name="prefix">
    ///     Only required if the file from which attachments are being copied has attachments with keys that
    ///     conflict with attachments already in the file. In this case, the specified prefix will be prepended to each key.
    ///     This affects only the key in the embedded files table, not the file name. The PDF specification doesn’t preclude
    ///     multiple attachments having the same file name.
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job CopyAttachmentsFrom(string file, string? prefix = null)
    {
        _copyAttachments ??= new List<CopyAttachment>();
        _copyAttachments.Add(new CopyAttachment(file, prefix));
        return this;
    }
    #endregion

    #region RemoveAttachment
    /// <summary>
    ///     Remove the specified attachment. This doesn’t only remove the attachment from the embedded files table but
    ///     also clears out the file specification to ensure that the attachment is actually not present in the output
    ///     file. That means that any potential internal links to the attachment will be broken. Run with <see cref="Verbose"/>
    ///     to see status of the removal. Use <see cref="ListAttachments"/> to find the attachment key. This option may
    ///     be repeated to remove multiple attachments.
    /// </summary>
    /// <param name="key">The key of the attachment</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job RemoveAttachment(string key)
    {
        _removeAttachment ??= new List<string>();
        _removeAttachment.Add(key);
        return this;
    }
    #endregion

    #region FlattenRotation
    /// <summary>
    ///     For each page that is rotated using the /Rotate key in the page’s dictionary, remove the /Rotate key and implement
    ///     the identical rotation semantics by modifying the page’s contents. This option can be useful to prepare files for
    ///     buggy PDF applications that don’t properly handle rotated pages. There is usually no reason to use this option
    ///     unless you are working around a specific problem.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job FlattenRotation()
    {
        _flattenRotation = string.Empty;
        return this;
    }
    #endregion

    #region FlattenAnnotations
    /// <summary>
    ///     This option collapses annotations into the pages’ contents with special handling for form fields. Ordinarily, an
    ///     annotation is rendered separately and on top of the page. Combining annotations into the page’s contents
    ///     effectively freezes the placement of the annotations, making them look right after various page transformations.
    ///     The library functionality backing this option was added for the benefit of programs that want to create n-up page
    ///     layouts and other similar things that don’t work well with annotations. In a PDF file, interactive form fields have
    ///     a value and, independently, a set of instructions, called an appearance, to render the filled-in field. If a form
    ///     is filled in by a program that doesn’t know how to update the appearances, they may become inconsistent with the
    ///     fields’ values. If qpdf detects this case, its default behavior is not to flatten those annotations because doing
    ///     so would cause the value of the form field to be lost. This gives you a chance to go back and re-save the form with
    ///     a program that knows how to generate appearances. qpdf itself can generate appearances with some limitations. See
    ///     the <see cref="GenerateAppearances" /> option for details.
    /// </summary>
    /// <param name="parameter">
    ///     <see cref="FlattenAnnotations" />
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job FlattenAnnotations(FlattenAnnotations parameter)
    {
        _flattenAnnotation = parameter;
        return this;
    }
    #endregion

    #region Rotate
    /// <summary>
    ///     Rotate the specified range of pages by the specified angle, which must be a multiple of 90 degrees
    /// </summary>
    /// <param name="angle">
    ///     <see cref="Rotation" />
    /// </param>
    /// <param name="pageRange">
    ///     The page range or <c>null</c>, see https://qpdf.readthedocs.io/en/stable/cli.html?highlight=ranges#page-ranges
    /// </param>
    /// <returns></returns>
    public Job Rotate(Rotation angle, string? pageRange = null)
    {
        var result = angle switch
        {
            Rotation.Rotate0 => "+0",
            Rotation.Rotate90 => "+90",
            Rotation.RotateMinus90 => "-90",
            Rotation.Rotate180 => "+180",
            Rotation.RotateMinus180 => "-180",
            Rotation.Rotate270 => "+270",
            Rotation.RotateMinus270 => "-270",
            _ => string.Empty
        };

        if (pageRange != null)
            result += $":{pageRange}";

        _rotate = result;
        return this;
    }
    #endregion

    #region GenerateAppearances
    /// <summary>
    ///     If a file contains interactive form fields and indicates that the appearances are out of date with the values of
    ///     the form, this flag will regenerate appearances, subject to a few limitations. Note that there is usually no reason
    ///     to do this, but it can be necessary before using the <see cref="FlattenAnnotations" /> option.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job GenerateAppearances()
    {
        _generateAppearances = string.Empty;
        return this;
    }
    #endregion

    #region OptimizeImages
    /// <summary>
    ///     This flag causes qpdf to recompress all images that are not compressed with DCT (JPEG) using DCT compression as
    ///     long as doing so decreases the size in bytes of the image data and the image does not fall below minimum specified
    ///     dimensions. Useful information is provided when used in combination with <see cref="Verbose" />. See also the
    ///     <see cref="OiMinWidth" />, <see cref="OiMinHeight" />, and <see cref="OiMinArea" /> options. By default, inline
    ///     images are converted to regular images and optimized as well. Use <see cref="KeepInlineImages"/> to prevent inline
    ///     images from being included.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job OptimizeImages()
    {
        _optimizeImages = string.Empty;
        return this;
    }
    #endregion

    #region OiMinWidth
    /// <summary>
    ///     Avoid optimizing images whose width is below the specified amount. If omitted, the default is <b>128</b> pixels. Use
    ///     <b>0</b> for no minimum.
    /// </summary>
    /// <param name="width">The width</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job OiMinWidth(int width = 128)
    {
        if (width < 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Needs to be a value of 0 or greater");

        _oiMinWidth = width.ToString();
        return this;
    }
    #endregion

    #region OiMinHeight
    /// <summary>
    ///     Avoid optimizing images whose height is below the specified amount. If omitted, the default is <b>128</b> pixels. Use
    ///     <b>0</b> for no minimum.
    /// </summary>
    /// <param name="height">The height</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job OiMinHeight(int height = 128)
    {
        if (height < 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Needs to be a value of 0 or greater");

        _oiMinHeight = height.ToString();
        return this;
    }
    #endregion

    #region OiMinArea
    /// <summary>
    ///     Avoid optimizing images whose pixel count (width × height) is below the specified amount. If omitted, the default
    ///     is <b>16384</b> pixels. Use <b>0</b> for no minimum.
    /// </summary>
    /// <param name="pixels">The amount of pixels</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job OiMinArea(int pixels = 16384)
    {
        if (pixels < 0)
            throw new ArgumentOutOfRangeException(nameof(pixels), "Needs to be a value of 0 or greater");

        _oiMinArea = pixels.ToString();
        return this;
    }
    #endregion

    #region KeepInlineImages
    /// <summary>
    ///     Prevent inline images from being included in image optimization done by <see cref="OptimizeImages" />.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job KeepInlineImages()
    {
        _keepInlineImages = string.Empty;
        return this;
    }
    #endregion

    #region RemovePageLabels
    /// <summary>
    ///     Exclude page labels (explicit page numbers) from the <see cref="OutputFile"/>.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job RemovePageLabels()
    {
        _removePageLabels = string.Empty;
        return this;
    }
    #endregion
    
    #region IsEncrypted
    /// <summary>
    ///     This option can be used for password-protected files even if you don’t know the password.
    ///     This option is useful for shell scripts.Other options are ignored if this is given.This option is mutually
    ///     exclusive with <see cref="RequiresPassword"/>.Both this option and <see cref="RequiresPassword"/> exit with
    ///     status <see cref="ExitCodeIsEncrypted.NotEncrypted"/> for non-encrypted
    ///     files.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job IsEncrypted()
    {
        _isEncrypted = string.Empty;
        return this;
    }
    #endregion

    #region RequiresPassword
    /// <summary>
    ///     Silently exit with a code indicating the file’s password status. If a password is supplied with the method
    ///     <see cref="InputFile" /> and the <see cref="Password"/> parameter set, that password is used to open the
    ///     file just as with any normal invocation of qpdf. That means that using this option can be used to check
    ///     the correctness of the password. This option is mutually exclusive with <see cref="IsEncrypted" />.
    /// </summary>
    /// <remarks>
    ///     Use with the method <see cref="Password" /> method
    /// </remarks>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job RequiresPassword()
    {
        _requiresPassword = string.Empty;
        return this;
    }
    #endregion

    #region Check
    /// <summary>
    ///     Check the file’s structure as well as encryption, linearization, and encoding of stream data, and write information
    ///     about the file to standard output. An exit status of 0 indicates syntactic correctness of the PDF file. Note that
    ///     <see cref="Check" /> writes nothing to standard error when everything is valid, so if you are using this to
    ///     programmatically validate files in bulk, it is safe to run without output redirected to /dev/null and just check
    ///     for a 0 exit code. A file for which <see cref="Check" /> reports no errors may still have errors in stream data
    ///     content or may contain constructs that don’t conform to the PDF specification, but it should be syntactically
    ///     valid. If <see cref="Check" /> reports any errors, qpdf will exit with a status of
    ///     <see cref="ExitCode.ErrorsFoundFileNotProcessed" />. There are some recoverable conditions that <see cref="Check" />
    ///     detects. These are issued as warnings instead of errors. If qpdf finds no errors but finds warnings, it will exit
    ///     with a status of <see cref="ExitCode.WarningsWereFoundFileProcessed" />. When <see cref="Check" /> is combined with
    ///     other options, checks are always performed before any other options are processed. For erroneous files,
    ///     <see cref="Check" /> will cause qpdf to attempt to recover, after which other options are effectively operating on
    ///     the recovered file. Combining <see cref="Check" /> with other options in this way can be useful for manually
    ///     recovering severely damaged files.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Check()
    {
        _check = string.Empty;
        return this;
    }
    #endregion

    #region ShowEncryption
    /// <summary>
    ///     This option shows document encryption parameters. It also shows the document’s user password if the owner password
    ///     is given and the file was encrypted using older encryption formats that allow user password recovery. (See PDF
    ///     Encryption for a technical discussion of this feature.) The output of <see cref="ShowEncryption" /> is included in
    ///     the output of <see cref="Check" />.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ShowEncryption()
    {
        _showEncryption = string.Empty;
        return this;
    }
    #endregion

    #region ShowEncryptionKey
    /// <summary>
    ///     When encryption information is being displayed, as when <see cref="Check" /> or <see cref="ShowEncryption" /> is
    ///     given, display the computed or retrieved encryption key as a hexadecimal string. This value is not ordinarily
    ///     useful to users, but it can be used as the parameter to <see cref="Password" /> if the <see cref="PasswordIsHexKey" />
    ///     is specified. Note that, when PDF files are encrypted, passwords and other metadata are used only to compute an encryption
    ///     key, and the encryption key is what is actually used for encryption. This enables retrieval of that key. See PDF Encryption
    ///     for a technical discussion.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ShowEncryptionKey()
    {
        _showEncryptionKey = string.Empty;
        return this;
    }
    #endregion

    #region CheckLinearization
    /// <summary>
    ///     Check to see whether a file is linearized and, if so, whether the linearization hint tables are correct. qpdf does
    ///     not check all aspects of linearization. A linearized PDF file with linearization errors that is otherwise correct
    ///     is almost always readable by a PDF viewer. As such, “errors” in PDF linearization are treated by qpdf as warnings.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job CheckLinearization()
    {
        _checkLinearization = string.Empty;
        return this;
    }
    #endregion

    #region ShowLinearization
    /// <summary>
    ///     Check and display all data in the linearization hint tables.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ShowLinearization()
    {
        _showLinearization = string.Empty;
        return this;
    }
    #endregion

    #region ShowXref
    /// <summary>
    ///     Show the contents of the cross-reference table or stream in a human-readable form. The cross-reference data gives
    ///     the offset of regular objects and the object stream ID and 0-based index within the object stream for compressed
    ///     objects. This is especially useful for files with cross-reference streams, which are stored in a binary format. If
    ///     the file is invalid and cross reference table reconstruction is performed, this option will show the information in
    ///     the reconstructed table.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ShowXref()
    {
        _showXref = string.Empty;
        return this;
    }
    #endregion

    #region ShowObject
    /// <summary>
    ///     Show the contents of the given object. This is especially useful for inspecting objects that are inside of object
    ///     streams (also known as “compressed objects”).
    /// </summary>
    /// <param name="obj">The object to show</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ShowObject(string obj)
    {
        _showObject = obj;
        return this;
    }
    #endregion

    #region RawStreamData
    /// <summary>
    ///     When used with <see cref="ShowObject" />, if the object is a stream, write the raw (compressed) binary stream data
    ///     to standard output instead of the object’s contents. Avoid combining this with other inspection options to avoid
    ///     commingling the stream data with other output. See also <see cref="FilteredStreamData" />.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job RawStreamData()
    {
        _rawStreamData = string.Empty;
        return this;
    }
    #endregion

    #region FilteredStreamData
    /// <summary>
    ///     When used with <see cref="ShowObject" />, if the object is a stream, write the filtered (uncompressed, potentially
    ///     binary) stream data to standard output instead of the object’s contents. If the stream is filtered using filters
    ///     that qpdf does not support, an error will be issued. This option acts as if <see cref="DecodeLevel" />
    ///     <see cref="Enums.DecodeLevel.All" /> was specified, so it will uncompress images compressed with supported
    ///     lossy compression schemes. Avoid combining this with other inspection options to avoid commingling the stream data
    ///     with other output. This option may be combined with <see cref="NormalizeContent" />.If you do this, qpdf will attempt
    ///     to run content normalization even if the stream is not a content stream, which will probably produce unusable results.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job FilteredStreamData()
    {
        _filteredStreamData = string.Empty;
        return this;
    }
    #endregion

    #region ShowNPages
    /// <summary>
    ///     Print the number of pages in the input file on a line by itself. Since the number of pages appears by itself on a
    ///     line, this option can be useful for scripting if you need to know the number of pages in a file.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ShowNPages()
    {
        _showNpages = string.Empty;
        return this;
    }
    #endregion

    #region ShowPages
    /// <summary>
    ///     Show the object and generation number for each page dictionary object and for each content stream associated with
    ///     the page. Having this information makes it more convenient to inspect objects from a particular page.
    /// </summary>
    /// <remarks>
    ///     See also <see cref="WithImages" />.
    /// </remarks>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ShowPages()
    {
        _showPages = string.Empty;
        return this;
    }
    #endregion

    #region WithImages
    /// <summary>
    ///     When used with <see cref="ShowPages" />, also shows the object and generation numbers for the image objects on each page.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job WithImages()
    {
        _withImages = string.Empty;
        return this;
    }
    #endregion

    #region ListAttachments
    /// <summary>
    ///     Show the key and stream number for each embedded file. With <see cref="Verbose" />, additional information,
    ///     including preferred file name, description, dates, and more are also displayed. The key is usually but not always
    ///     equal to the file name and is needed by some of the other options.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ListAttachments()
    {
        _listAttachments = string.Empty;
        return this;
    }
    #endregion

    #region ShowAttachment
    /// <summary>
    ///     Write the contents of the specified attachment to standard output as binary data. The key should match one of the
    ///     keys shown by <see cref="ListAttachments"/>. If this option is given more than once, only the last attachment will be shown.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ShowAttachment(string key)
    {
        _showAttachment = key;
        return this;
    }
    #endregion

    #region Json
    /// <summary>
    ///     Generate a JSON representation of the file. This is described in depth in QPDF JSON. The version parameter can be
    ///     used to specify which version of the qpdf JSON format should be output. The only supported value is 1, but it’s
    ///     possible that a new JSON output version will be added in a future version. You can also specify latest to use the
    ///     latest JSON version. For backward compatibility, the default value will remain 1 until qpdf version 11, after which
    ///     point it will become latest. In all case, you can tell what version of the JSON output you have from the "version"
    ///     key in the output.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Json()
    {
        _json = string.Empty;
        return this;
    }
    #endregion

    #region JsonKey
    /// <summary>
    ///     This option is repeatable. If given, only the specified top-level keys will be included in the JSON output.
    ///     Otherwise, all keys will be included. <b>version</b> and <b>parameters</b> will always appear in the output.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job JsonKey(string key)
    {
        _jsonKey ??= new List<string>();
        _jsonKey.Add(key);
        return this;
    }
    #endregion

    #region JsonObject
    /// <summary>
    ///     This option is repeatable. If given, only specified objects will be shown in the “objects” key of the JSON output.
    ///     Otherwise, all objects will be shown.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job JsonObject(string obj)
    {
        _jsonObject ??= new List<string>();
        _jsonObject.Add(obj);
        return this;
    }
    #endregion

    #region StaticId
    /// <summary>
    ///     Use a fixed value for the document ID (/ID in the trailer). This is intended for testing only. <b>Never</b> use it for
    ///     production files. If you are trying to get the same ID each time for a given file and you are not generating encrypted files,
    ///     consider using the <see cref="DeterministicId"/> option.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job StaticId()
    {
        _staticId = string.Empty;
        return this;
    }
    #endregion

    #region StaticAesIv
    /// <summary>
    ///     Use a static initialization vector for AES-CBC. This is intended for testing only so that output files can be reproducible.
    ///     <b>Never</b> use it for production files. This option in particular is not secure since it significantly weakens the encryption.
    ///     When combined with <see cref="StaticId"/> and using the three-step process described in Idempotency, it is possible to create
    ///     byte-for-byte idempotent output with PDF files that use 256-bit encryption to assist with creating reproducible test suites.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job StaticAesIv()
    {
        _staticAesIv = string.Empty;
        return this;
    }
    #endregion

    #region LinearizePass1
    /// <summary>
    ///     Use a static initialization vector for AES-CBC. This is intended for testing only so that output files can be reproducible.
    ///     <b>Never</b> use it for production files. This option in particular is not secure since it significantly weakens the encryption.
    ///     When combined with <see cref="StaticId"/> and using the three-step process described in Idempotency, it is possible to create
    ///     byte-for-byte idempotent output with PDF files that use 256-bit encryption to assist with creating reproducible test suites.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job LinearizePass1(string fileName)
    {
        _linearizePass1 = fileName;
        return this;
    }
    #endregion

    #region InternalRun
    /// <summary>
    ///     Runs the job with the given parameters
    /// </summary>
    /// <param name="output">Returns any output that is generated by qpdf</param>
    private int InternalRun(out string? output)
    {
        var settings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        settings.Converters.Add(new StringEnumConverter());

        var json = JsonConvert.SerializeObject(this, settings);
        var result = QPdfApi.Native.RunFromJSONWithResult(json, out var outPointer, out var errorPointer);
        var outResult = Marshal.PtrToStringAnsi(outPointer);
        var errorResult = Marshal.PtrToStringAnsi(errorPointer);

        outResult = outResult?.Trim();
        errorResult = errorResult?.Trim();

        Marshal.FreeCoTaskMem(outPointer);
        Marshal.FreeCoTaskMem(errorPointer);

        output = outResult;

        if (!string.IsNullOrWhiteSpace(errorResult))
        {
            if (string.IsNullOrWhiteSpace(output))
                output = errorResult;
            else
                output += Environment.NewLine + errorResult;
        }

        return result;
    }
    #endregion

    #region Run
    /// <summary>
    ///     Runs the <see cref="Job" /> with the given parameters
    /// </summary>
    /// <param name="output">Returns any output that is generated by qpdf</param>
    /// <returns>
    ///     <see cref="ExitCode" />
    /// </returns>
    public ExitCode Run(out string? output)
    {
        if (_isEncrypted == string.Empty)
            throw new ArgumentException("Use the method 'RunIsEncrypted' when the IsEncrypted method is used");

        if (_requiresPassword == string.Empty)
            throw new ArgumentException(
                "Use the method 'RunRequiresPassword' when the RequiresPassword method is used");

        return (ExitCode)InternalRun(out output);
    }
    #endregion

    #region RunIsEncrypted
    /// <summary>
    ///     Runs the <see cref="Job" /> with the given parameters
    /// </summary>
    /// <param name="output">Returns any output that is generated by qpdf</param>
    /// <returns>
    ///     <see cref="ExitCode" />
    /// </returns>
    public ExitCodeIsEncrypted RunIsEncrypted(out string? output)
    {
        if (_requiresPassword == string.Empty)
            throw new ArgumentException(
                "Use the method 'RunRequiresPassword' when the RequiresPassword method is used");

        return (ExitCodeIsEncrypted)InternalRun(out output);
    }
    #endregion

    #region RunRequiresPassword
    /// <summary>
    ///     Runs the <see cref="Job" /> with the given parameters
    /// </summary>
    /// <param name="output">Returns any output that is generated by qpdf</param>
    /// <returns>
    ///     <see cref="ExitCode" />
    /// </returns>
    public ExitCodeRequiresPassword RunRequiresPassword(out string? output)
    {
        if (_isEncrypted == string.Empty)
            throw new ArgumentException("Use the method 'RunIsEncrypted' when the IsEncrypted method is used");

        return (ExitCodeRequiresPassword)InternalRun(out output);
    }
    #endregion
}