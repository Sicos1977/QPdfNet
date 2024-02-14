//
// Job.cs
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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
public class Job : IDisposable
{
    #region Delegates
    private delegate int CallbackDelegate(IntPtr data, int length, IntPtr udata);
    #endregion

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
    [JsonProperty("removeRestrictions")] private string? _removeRestrictions;
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
    [JsonProperty("setPageLabels")] private string? _setPageLabels;
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
    [JsonProperty("jsonHelp")] private string? _jsonHelp;
    [JsonProperty("jsonStreamData")] private string? _jsonStreamData;
    [JsonProperty("jsonStreamPrefix")] private string? _jsonStreamPrefix;
    [JsonProperty("jsonKey")] private List<string>? _jsonKey;
    [JsonProperty("jsonObject")] private List<string>? _jsonObject;
    [JsonProperty("jsonOutput")] private string? _jsonOutput;
    [JsonProperty("jsonInput")] private string? _jsonInput;
    [JsonProperty("updateFromJson")] private string? _updateFromJson;
    [JsonProperty("staticId")] private string? _staticId;
    [JsonProperty("staticAesIv")] private string? _staticAesIv;
    [JsonProperty("linearizePass1")] private string? _linearizePass1;

    private static readonly IQPdfApiSignatures Native = new QPdfApi().Native;

    private readonly IntPtr _loggerHandle;

    private static readonly CallbackDelegate LoggingCallbackDelegate = LoggingCallback;
    private static readonly IntPtr LoggingCallbackPointer = Marshal.GetFunctionPointerForDelegate(LoggingCallbackDelegate);
    private static readonly CallbackDelegate SaveCallbackDelegate = SaveCallback;
    private static readonly IntPtr SaveCallbackPointer = Marshal.GetFunctionPointerForDelegate(SaveCallbackDelegate);

    // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
    // Free() does not work, if readonly.
    private GCHandle _infoHandle;
    private GCHandle _warnHandle;
    private GCHandle _errorHandle;
    private GCHandle _saveHandle;

    private readonly StringBuilder _info;
    private readonly StringBuilder _warn;
    private readonly StringBuilder _error;
    private readonly StringBuilder _output;
    private readonly List<byte> _save;
    // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable
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

        _info = new StringBuilder();
        _warn = new StringBuilder();
        _error = new StringBuilder();
        _output = new StringBuilder();
        _save = new List<byte>();

        _loggerHandle = Native.qpdflogger_create();

        _infoHandle = GCHandle.Alloc(_info);
        var infoPtr = GCHandle.ToIntPtr(_infoHandle);
        Native.qpdflogger_set_info(loggerHandle: _loggerHandle, destination: qpdf_log_dest_e.qpdf_log_dest_custom, callBackHandler: Job.LoggingCallbackPointer, udata: infoPtr);

        _warnHandle = GCHandle.Alloc(_warn);
        var warnPtr = GCHandle.ToIntPtr(_warnHandle);
        Native.qpdflogger_set_warn(loggerHandle: _loggerHandle, destination: qpdf_log_dest_e.qpdf_log_dest_custom, callBackHandler: Job.LoggingCallbackPointer, udata: warnPtr);

        _errorHandle = GCHandle.Alloc(_error);
        var errorPtr = GCHandle.ToIntPtr(_errorHandle);
        Native.qpdflogger_set_error(loggerHandle: _loggerHandle, destination: qpdf_log_dest_e.qpdf_log_dest_custom, callBackHandler: Job.LoggingCallbackPointer, udata: errorPtr);

        _saveHandle = GCHandle.Alloc(_save);
        var savePtr = GCHandle.ToIntPtr(_saveHandle);
        Native.qpdflogger_set_save(loggerHandle: _loggerHandle, destination: qpdf_log_dest_e.qpdf_log_dest_custom, callBackHandler: Job.SaveCallbackPointer, udata: savePtr);
    }
    #endregion

    #region Callbacks
    private static int LoggingCallback(IntPtr data, int length, IntPtr udata)
    {
        var bytes = new byte[length];
        Marshal.Copy(data, bytes, 0, length);

        var handle = GCHandle.FromIntPtr(udata);
        var stringBuilder = (StringBuilder)handle.Target;
        stringBuilder.Append(Encoding.ASCII.GetString(bytes));

        return 0;
    }

    private static int SaveCallback(IntPtr data, int length, IntPtr udata)
    {
        var bytes = new byte[length];
        Marshal.Copy(data, bytes, 0, length);

        var handle = GCHandle.FromIntPtr(udata);
        var save = (List<byte>)handle.Target;
        save.AddRange(bytes);

        return 0;
    }
    #endregion

    #region Reset
    /// <summary>
    ///     Resets the job to its default state
    /// </summary>
    private void Reset()
    {
        _inputFile = null;
        _outputFile = null;
        _empty = null;
        _replaceInput = null;
        _warningExit0 = null;
        _password = null;
        _passwordFile = null;
        _verbose = null;
        _noWarn = null;
        _deterministicId = null;
        _allowWeakCrypto = null;
        _keepFilesOpen = null;
        _keepFilesOpenThreshold = null;
        _passwordIsHexKey = null;
        _suppressPasswordRecovery = null;
        _passwordMode = default;
        _suppressRecovery = null;
        _ignoreXrefStreams = null;
        _linearize = null;
        _encryption = null;
        _decrypt = null;
        _removeRestrictions = null;
        _copyEncryption = null;
        _encryptionFilePassword = null;
        _qdf = null;
        _noOriginalObjectIds = null;
        _compressStreams = null;
        _decodeLevel = default;
        _streamData = default;
        _recompressFlate = null;
        _compressionLevel = null;
        _normalizeContent = null;
        _objectStreams = default;
        _preserveUnreferenced = null;
        _removeUnreferencedResources = default;
        _preserveUnreferencedResources = null;
        _newlineBeforeEndstream = null;
        _coalesceContents = null;
        _externalizeInlineImages = null;
        _iiMinBytes = null;
        _minVersion = null;
        _forceVersion = null;
        _collate = null;
        _pages = null;
        _splitPages = null;
        _overlay = null;
        _underlay = null;
        _addAttachment = null;
        _copyAttachments = null;
        _removeAttachment = null;
        _flattenRotation = null;
        _flattenAnnotation = default;
        _rotate = null;
        _generateAppearances = null;
        _optimizeImages = null;
        _oiMinWidth = null;
        _oiMinHeight = null;
        _oiMinArea = null;
        _keepInlineImages = null;
        _removePageLabels = null;
        _setPageLabels = null;
        _isEncrypted = null;
        _requiresPassword = null;
        _check = null;
        _showEncryption = null;
        _showEncryptionKey = null;
        _checkLinearization = null;
        _showLinearization = null;
        _showXref = null;
        _showObject = null;
        _rawStreamData = null;
        _filteredStreamData = null;
        _showNpages = null;
        _showPages = null;
        _withImages = null;
        _listAttachments = null;
        _showAttachment = null;
        _json = null;
        _jsonHelp = null;
        _jsonStreamData = null;
        _jsonStreamPrefix = null;
        _jsonKey = null;
        _jsonObject = null;
        _jsonOutput = null;
        _jsonInput = null;
        _updateFromJson = null;
        _staticId = null;
        _staticAesIv = null;
        _linearizePass1 = null;
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
        Logger.LogInformation($"Writing output to PDF file '{fileName}'");
        _outputFile = fileName;
        return this;
    }
    #endregion

    #region Empty
    /// <summary>
    ///     This option may be given in place of infile. This causes qpdf to use a dummy input file that contains zero pages.
    ///     This option is useful in conjunction with <see cref="Pages"/>. See <see cref="Pages"/> for details.
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
    ///     This option may be given in place of <see cref="OutputFile"/>. This causes qpdf to replace the <see cref="InputFile"/>
    ///     file with the <see cref="OutputFile"/>. It does this by writing to infilename. ~qpdf-temp# and, when done, overwriting
    ///     the <see cref="InputFile"/> with the temporary file. If there were any warnings, the original input is saved as infilename.
    ///     ~qpdf-orig. If there are errors, the <see cref="InputFile"/> is left untouched.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ReplaceInput()
    {
        Logger.LogInformation(string.IsNullOrEmpty(_inputFile)
            ? $"Replacing input PDF file '{_inputFile}'"
            : "Replacing input PDF file");

        _replaceInput = string.Empty;
        return this;
    }
    #endregion

    #region WarningExit0
    /// <summary>
    ///     If there were warnings only and no errors, exit with exit code <see cref="ExitCode.Success" /> instead of
    ///     <see cref="ExitCode.WarningsWereFoundFileProcessed" />. When combined with <see cref="NoWarn" />, the effect
    ///     is for qpdf to completely ignore warnings.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job WarningExit0()
    {
        Logger.LogInformation($"If there were warnings only and no errors, exit with exit code '{ExitCode.Success}' instead of '{ExitCode.WarningsWereFoundFileProcessed}'");
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
        Logger.LogInformation($"Setting PDF password to '{new string('*', password.Length)}'");
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
        Logger.LogInformation($"Setting password file to '{file}' with on the first line the password of the PDF");

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
    ///     code <see cref="ExitCode.WarningsWereFoundFileProcessed" />. To completely ignore warnings, also specify
    ///     <see cref="WarningExit0" />. Use with caution as qpdf is not always successful in recovering from situations that
    ///     cause warnings to be issued.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job NoWarn()
    {
        Logger.LogInformation("Suppress writing of warnings to output");
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
            Logger.LogInformation("Keeping PDF files open");

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
        {
            Logger.LogError($"KeepFilesOpenThreshold '{count}' needs to be a value of 1 or greater");
            throw new ArgumentOutOfRangeException(nameof(count), "Needs to be a value of 1 or greater");
        }

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
    ///     used to open the main input file. It does not apply to other files opened by <see cref="Pages"/> or other options
    ///     or to files being written. Most users will never have a need for this option, and no standard viewers support this
    ///     mode of operation, but it can be useful for forensic or investigatory purposes. For example, if a PDF file is
    ///     encrypted with an unknown password, a brute-force attack using the key directly is sometimes more efficient than
    ///     one using the password.Also, if a file is heavily damaged, it may be possible to derive the encryption key and
    ///     recover parts of the file using it directly.To expose the encryption key used by an encrypted file that you can open
    ///     normally, use the <see cref="ShowEncryptionKey" /> option.
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
    ///     Create linearized (web-optimized) <see cref="OutputFile"/>s. Linearized files are formatted in a way that allows
    ///     compliant readers to begin displaying a PDF file before it is fully downloaded. Ordinarily, the entire file must
    ///     be present before it can be rendered because important cross-reference information typically appears at the end
    ///     of the file.
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
    ///     Create a <see cref="OutputFile"/> with no encryption even if the input file is encrypted. This option overrides
    ///     the default behavior of preserving whatever encryption was present on the <see cref="InputFile"/>. This functionality
    ///     is not intended to be used for bypassing copyright restrictions or other restrictions placed on files by their producers.
    ///     See also <see cref="CopyEncryption"/>.
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

    #region RemoveRestrictions
    /// <summary>
    ///     Remove security restrictions associated with digitally signed PDF files. This may be combined with <see cref="Decrypt"/> to
    ///     allow free editing of previously signed/encrypted files. This option invalidates the signature but leaves its visual appearance intact.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job RemoveRestrictions()
    {
        Logger.LogInformation("Creating PDF output file with security restrictions associated with digitally signed PDF files removed");
        _removeRestrictions = string.Empty;
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
            var message = $"The PDF file '{file}' could not be found";
            Logger.LogError(message);
            throw new FileNotFoundException(message);
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
    ///     To suppress this behavior and leave streams uncompressed, use <paramref name="compress" /> = <c>false</c>.
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
            var message = $"Compression level '{level}' should be in the range 1 to 9";
            Logger.LogError(message);
            throw new ArgumentOutOfRangeException(nameof(level), message);
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
        Logger.LogInformation($"Setting removing unreferenced resources to '{removeUnreferencedResources}'");
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
        Logger.LogInformation("Setting removing unreferenced resources to 'Auto'");
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
        Logger.LogInformation("Inserting a newline before the endstream keyword, not counted in the length, after any stream content even if the last character of the stream was a newline");
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
        Logger.LogInformation("Coalescing content to a single stream");
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
        Logger.LogInformation("Convert inline images to regular images");
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
        {
            var message = $"IiMinBytes '{size}' should be 0 (for no minimum) or a value greater then 0";
            Logger.LogError(message);
            throw new ArgumentOutOfRangeException(nameof(size), message);
        }

        Logger.LogInformation($"Avoiding converting inline images whose size is below {size} bytes to regular images");

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
        {
            var message = $"MinVersion '{version}' can't be null or white space";
            Logger.LogError(message);
            throw new ArgumentOutOfRangeException(nameof(version), message);
        }

        Logger.LogInformation($"Forcing the PDF output file to be at least version '{version}'");

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
        {
            var message = $"ForceVersion '{version}' can't be null or white space";
            Logger.LogError(message);
            throw new ArgumentOutOfRangeException(nameof(version), message);
        }

        Logger.LogInformation($"Forcing the PDF output file to be the exact version '{version}' even when the file may have content that is not supported in this version");
        
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

        Logger.LogInformation($"Setting page selection to file '{file}' with range '{range}' and password '{(password?.Length > 0 ? new string('*', password.Length) : string.Empty)}'");

        _pages.Add(new Pages(file, range, password));
        return this;
    }
    #endregion

    #region Collate
    /// <summary>
    ///     This option causes qpdf to collate rather than concatenate pages specified with <see cref="Pages"/>. With a numeric parameter,
    ///     collate in groups of <paramref name="n" />. The default is <b>1</b>. With comma-separated numeric parameters, take n from the
    ///     first file, m from the second, etc.
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
        {
            var message = "$Collate value '{n}' should be greater than zero";
            Logger.LogError(message);
            throw new ArgumentOutOfRangeException(nameof(n), message);
        }

        Logger.LogError($"$Collating pages with value '{n}'");
        _collate = n.ToString();
        return this;
    }

    /// <summary>
    ///     This option causes qpdf to collate rather than concatenate pages specified with <see cref="Pages"/>. With a numeric parameter,
    ///     collate in groups of <paramref name="pages" />. The default is <b>1</b>. With comma-separated numeric parameters, take n from the
    ///     first file, m from the second, etc.
    /// </summary>
    /// <param name="pages">
    ///     A list of pages to take n from the first file, m from the second, etc.
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Collate(List<int> pages)
    {
        foreach (var page in pages)
        {
            if (page <= 0)
            {
                var message = "$Collate value '{n}' should be greater than zero";
                Logger.LogError(message);
                throw new ArgumentOutOfRangeException(nameof(page), message);
            }
        }

        var n = string.Join(",", pages);
        Logger.LogError($"$Collating pages with values '{n}'");
        _collate = n;
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
        {
            var message = $"SplitPages value '{n}' should be greater than zero";
            Logger.LogError(message);
            throw new ArgumentOutOfRangeException(nameof(n), message);
        }

        Logger.LogError($"Splitting pages in groups of '{n}' page{(n == 1 ? "s" : string.Empty)}");
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
        {
            var message = $"The overlay PDF file '{file}' could not be found";
            Logger.LogError(message);
            throw new FileNotFoundException(message);
        }

        _overlay = new Dictionary<string, string> { { "file", file } };

#pragma warning disable CS8604
        if (!string.IsNullOrWhiteSpace(to))
            _overlay.Add("to", to);

        if (!string.IsNullOrWhiteSpace(from))
            _overlay.Add("from", from);

        if (!string.IsNullOrWhiteSpace(repeat))
            _overlay.Add("repeat", repeat);
#pragma warning restore CS8604

        Logger.LogInformation($"Overlaying PDF file '{file}' from '{from}' to '{to}' and repeat '{repeat}'");

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
        {
            var message = $"The underlay PDF file '{file}' could not be found";
            Logger.LogError(message);
            throw new FileNotFoundException(message);
        }

        _underlay = new Dictionary<string, string> { { "file", file } };

#pragma warning disable CS8604
        if (!string.IsNullOrWhiteSpace(to))
            _underlay.Add("to", to);

        if (!string.IsNullOrWhiteSpace(from))
            _underlay.Add("from", from);

        if (!string.IsNullOrWhiteSpace(repeat))
            _underlay.Add("repeat", repeat);
#pragma warning restore CS8604

        Logger.LogInformation($"Under-laying PDF file '{file}' from '{from}' to '{to}' and repeat '{repeat}'");

        return this;
    }
    #endregion

    #region AddAttachment
    /// <summary>
    ///     This flag starts add attachment options, which are used to add attachments to a file.
    ///     The  flag and its options may be repeated to add multiple attachments
    /// </summary>
    /// <param name="file">
    ///     Specify the file with it's full path to be used for the attachment.
    /// </param>
    /// <param name="description">Supply descriptive text for the attachment, displayed by some PDF viewers.</param>
    /// <param name="replace">
    ///     Indicate that any existing attachment with the same key should be replaced by the new attachment.
    ///     Otherwise, qpdf gives an error if an attachment with that key is already present.
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    /// <remarks>
    ///     The key, file name, creation date, modify date and mime type will be read from the given <paramref name="file"/>.
    ///     If you want to set them your self then use the other AddAttachment method
    /// </remarks>
    /// <exception cref="FileNotFoundException"></exception>
    public Job AddAttachment(string file, string? description = null, bool replace = false)
    {
        if (!System.IO.File.Exists(file))
        {
            var message = $"The attachment file '{file}' could not be found";
            Logger.LogError(message);
            throw new FileNotFoundException(message);
        }

        var addAttachment = new AddAttachment(file, description, replace);

        Logger.LogInformation($"Adding attachment '{file}', with description '{description}' and replace '{replace}'");

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
    /// <param name="modDate">Specify the attachment’s modification date in PDF format; defaults to the current time.</param>
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
        DateTime modDate,
        string mimeType,
        string? description = null,
        bool replace = false)
    {
        if (!System.IO.File.Exists(file))
        {
            var message = $"The attachment file '{file}' could not be found";
            Logger.LogError(message);
            throw new FileNotFoundException(message);
        }

        var addAttachment = new AddAttachment(file, key, fileName, creationDate, modDate, mimeType, description: description, replace: replace);

        Logger.LogInformation($"Adding attachment '{file}', with key '{key}', file name '{fileName}', creation date '{creationDate}', modify date '{modDate}', mime type '{mimeType}' description '{description}' and replace '{replace}'");

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
        if (!System.IO.File.Exists(file))
        {
            var message = $"The copy from attachment PDF file '{file}' could not be found";
            Logger.LogError(message);
            throw new FileNotFoundException(message);
        }

        _copyAttachments ??= new List<CopyAttachment>();
        _copyAttachments.Add(new CopyAttachment(file, prefix));

        Logger.LogInformation($"Copying attachments from PDF file '{file}' with prefix '{prefix}'");

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

        Logger.LogInformation($"Removing attachment with key '{key}'");

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
        Logger.LogInformation("Flatten rotation");
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
        Logger.LogInformation("Flatten annotations");
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

        Logger.LogInformation($"Rotating pages with angle '{angle}' and page range '{pageRange}'");

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
        Logger.LogInformation("Regenerate appearances");
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
        Logger.LogInformation("Optimizing all images that are not compressed with DCT (JPEG)");
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
        {
            var message = $"OiMinWidth '{width}' needs to be a value of 0 or greater";
            Logger.LogError(message);
            throw new ArgumentOutOfRangeException(nameof(width), message);
        }

        Logger.LogInformation($"Avoiding optimizing images whose width is below '{width}' bytes");

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
        {
            var message = $"OiMinHeight '{height}' needs to be a value of 0 or greater";
            Logger.LogError(message);
            throw new ArgumentOutOfRangeException(nameof(height), message);
        }

        Logger.LogInformation($"Avoiding optimizing images whose height is below '{height}' bytes");

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
        {
            var message = $"OiMinArea '{pixels}' needs to be a value of 0 or greater";
            Logger.LogError(message);
            throw new ArgumentOutOfRangeException(nameof(pixels), message);
        }

        Logger.LogInformation($"Avoiding optimizing images whose pixel count (width x height) is below '{pixels}'");

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
        Logger.LogInformation("Prevent inline images from being included in image optimization done by 'OptimizeImages'");
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
        Logger.LogInformation("Exclude page labels (explicit page numbers) from the output file");
        _removePageLabels = string.Empty;
        return this;
    }
    #endregion
    
    #region SetPageLabels
    /// <summary>
    ///     Set page labels (explicit page numbers) for the entire file.<br/>
    ///     A PDF file’s pages can be explicitly numbered using page labels.<br/>
    ///     Page labels in a PDF file have an optional type (Arabic numerals, upper/lower-case alphabetic characters, upper/lower-case Roman numerals),<br/>
    ///     an optional prefix, and an optional starting value, which defaults to 1.<br/>
    ///     A qpdf page label spec has the form<br/>
    ///     <br/>
    ///     <c>first-page:[type][/start[/prefix]]</c><br/>
    ///     <br/>
    ///     where<br/>
    ///     <br/>
    ///     first-page represents a sequential page number using the same format as page ranges (see Page Ranges): a number,<br/>
    ///     a number preceded by r to indicate counting from the end, or z indicating the last page<br/>
    ///     <br/>
    ///     type may be one of<br/>
    ///     <br/>
    ///         - <b>D</b>: Arabic numerals (digits)<br/>
    ///         - <b>A</b>: Upper-case alphabetic characters<br/>
    ///         - <b>a</b>: Lower-case alphabetic characters<br/>
    ///         - <b>R</b>: Upper-case Roman numerals<br/>
    ///         - <b>r</b>: Lower-case Roman numerals<br/>
    ///     omitted: the page number does not appear, though the prefix, if specified will still appear<br/>
    ///     <br/>
    ///     <c>prefix</c> may be any string and is prepended to each page label<br/>
    ///     <br/>
    ///     A given page label spec causes pages to be numbered according to that scheme starting with <c>first-page</c><br/>
    ///     and continuing until the next label spec or the end of the document.<br/>
    ///     If you want to omit numbering starting at a certain page, you can use first-page: as the spec.<br/>
    ///     <br/>
    ///     Here are some example page labeling schemes. First these examples, assume a 50-page document.<br/>
    ///     <br/>
    ///     <c>1:a 5:D</c><br/>
    ///     - The first four pages will be numbered <c>a</c> through <c>d</c>, then the remaining pages will numbered <c>1</c> through <c>46</c>.<br/><br/>
    ///     <c>1:r 5:D 12: 14:D/10 r5:D//A- z://"end note"</c>:<br/>
    ///     - The first four pages are numbered <c>i</c> through <c>iv</c><br/>
    ///     - The 5th page is numbered <c>1</c>, and pages are numbered sequentially through the 11th page, which will be numbered <c>7</c><br/>
    ///     - The 12th and 13th pages will not have labels<br/>
    ///     - The 14th page is numbered <c>10</c>. Pages will be numbered sequentially up through the 45th page, which will be numbered <c>41</c><br/>
    ///     - Starting with the 46th page (the fifth to last page) and going to the 49th page, pages will be labeled A-1 through A-4<br/>
    ///     - The 50th page (the last page) will be labeled <c>end note</c>.<br/>
    ///     <br/>
    ///     The limitations on the range of formats for page labels are as specified in Section 12.4.2 of the PDF spec, ISO 32000.<br/>
    ///     <seealso cref="RemovePageLabels"/>
    /// </summary>
    /// <param name="pageLabels">The page labels</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job SetPageLabels(string pageLabels)
    {
        if (string.IsNullOrWhiteSpace(pageLabels))
        {
            var message = "The page labels cannot be empty";
            Logger.LogError(message);
            throw new ArgumentException(message, nameof(pageLabels));
        }

        Logger.LogInformation($"Setting page labels to '{pageLabels}'");
        _setPageLabels = pageLabels;
        return this;
    }
    #endregion

    #region IsEncrypted
    /// <summary>
    ///     This option can be used for password-protected files even if you don’t know the password.
    ///     This option is useful for shell scripts. Other options are ignored if this is given. This option is mutually
    ///     exclusive with <see cref="RequiresPassword"/>. Both this option and <see cref="RequiresPassword"/> exit with
    ///     status <see cref="ExitCodeIsEncrypted.NotEncrypted"/> for non-encrypted
    ///     files.
    /// </summary>
    /// <remarks>
    ///     Use <see cref="RunIsEncrypted"/> instead of <see cref="Run(out string?)"/> when using this method
    /// </remarks>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job IsEncrypted()
    {
        Logger.LogInformation("Checking if the input file is encrypted");
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
        Logger.LogInformation($"Silently exit with a '{nameof(ExitCodeIsEncrypted)}' indicating the file’s password status");
        _requiresPassword = string.Empty;
        return this;
    }
    #endregion

    #region Check
    /// <summary>
    ///     Check the file’s structure as well as encryption, linearization, and encoding of stream data, and write information
    ///     about the file to standard output. An exit status of <see cref="ExitCode.Success" /> indicates syntactic correctness
    ///     of the PDF file. Note that <see cref="Check" /> writes nothing to standard error when everything is valid, so if you
    ///     are using this to programmatically validate files in bulk, it is safe to run without output redirected to /dev/null
    ///     and just check for a <see cref="ExitCode.Success" />. A file for which <see cref="Check" /> reports no errors may
    ///     still have errors in stream data content or may contain constructs that don’t conform to the PDF specification, but
    ///     it should be syntactically valid. If <see cref="Check" /> reports any errors, qpdf will exit with a status of
    ///     <see cref="ExitCode.ErrorsFoundFileNotProcessed" />. There are some recoverable conditions that <see cref="Check" />
    ///     detects. These are issued as warnings instead of errors. If qpdf finds no errors but finds warnings, it will exit
    ///     with a status of <see cref="ExitCode.WarningsWereFoundFileProcessed" />. When <see cref="Check" /> is combined with
    ///     other options, checks are always performed before any other options are processed. For erroneous files,
    ///     <see cref="Check" /> will cause qpdf to attempt to recover, after which other options are effectively operating on
    ///     the recovered file. Combining <see cref="Check" /> with other options in this way can be useful for manually
    ///     recovering severely damaged files.
    /// </summary>
    /// <remarks>
    ///     Use the <see cref="Output.Helper.ParseCheck"/> method to help parse the output
    /// </remarks>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Check()
    {
        Logger.LogInformation("Checking the file’s structure as well as encryption, linearization, and encoding of stream data, and write information about the file to output");
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
    /// <remarks>
    ///     Use the <see cref="Output.Helper.ParseShowEncryption"/> method to help parse the output
    /// </remarks>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ShowEncryption()
    {
        Logger.LogInformation("Showing document encryption parameters");
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
    /// <remarks>
    ///     Use the <see cref="Output.Helper.ParseShowEncryption"/> method to help parse the output
    /// </remarks>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ShowEncryptionKey()
    {
        Logger.LogInformation("Displaying the computed or retrieved encryption key as a hexadecimal string");
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
        Logger.LogInformation("Checking to see whether a file is linearized and, if so, whether the linearization hint tables are correct");
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
        Logger.LogInformation("Check and display all data in the linearization hint tables");
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
        Logger.LogInformation("Showing the contents of the cross-reference table or stream in a human-readable form");
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
        Logger.LogInformation($"Showing the contents of object '{obj}'");
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
        Logger.LogInformation("Writing the raw (compressed) binary stream data to output instead of the object’s contents");
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
        Logger.LogInformation("Writing the filtered (uncompressed, potentially binary) stream data to standard output instead of the object’s contents");
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
        Logger.LogInformation("Printing the number of pages in the input file on a line by itself");
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
        Logger.LogInformation("Showing the object and generation number for each page dictionary object and for each content stream associated with the page");
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
        Logger.LogInformation("Also shows the object and generation numbers for the image objects on each page");
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
    /// <remarks>
    ///     Use the <see cref="Output.Helper.ParseListAttachments"/> method to help parse the attachment names out of the output
    /// </remarks>
    public Job ListAttachments()
    {
        Logger.LogInformation("Listing attachments");
        _listAttachments = string.Empty;
        return this;
    }
    #endregion

    #region ShowAttachment
    /// <summary>
    ///     Write the contents of the specified attachment as binary data. The key should match one of the
    ///     keys shown by <see cref="ListAttachments"/>. If this option is given more than once, only the
    ///     last attachment will be shown.
    /// </summary>
    /// <param name="key">The key of the attachment to show</param>
    /// <remarks>
    ///     Use <see cref="Run(out string?, out byte[])"/> instead of <see cref="Run(out string?)"/> when using this method 
    /// </remarks>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ShowAttachment(string key)
    {
        Logger.LogInformation($"Showing attachment with key '{key}'");
        _showAttachment = key;
        return this;
    }
    #endregion

    #region Json
    /// <summary>
    ///     Generate a JSON representation of the file. This is described in depth in qpdf JSON. The version parameter can be used to
    ///     specify which version of the qpdf JSON format should be output. The version number be a number or latest. The default is
    ///     latest. As of qpdf 11, the latest version is 2. If you have code that reads qpdf JSON output, you can tell what version
    ///     of the JSON output you have from the "version" key in the output. Use the --json-help option to get a description of the
    ///     JSON object. Starting with qpdf 11, when this option is specified, an output file is optional (for backward compatibility)
    ///     and defaults to standard output. You may specify an output file to write the JSON to a file rather than standard output
    /// </summary>
    /// <param name="version"><see cref="JsonVersion"/></param>
    /// <remarks>
    ///     Use the <see cref="Info.Pdf"/> class if you want to get a nice object orientated output
    /// </remarks>
    /// <remarks>
    ///     Use <see cref="Run(out string?, out byte[])"/> instead of <see cref="Run(out string?)"/> when using this method 
    /// </remarks>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job Json(JsonVersion version = JsonVersion.Version2)
    {
        Logger.LogInformation($"Generating a JSON {version} representation of the input PDF file");
        _json = ((int)version).ToString();
        return this;
    }
    #endregion

    //#region JsonHelp
    ///// <summary>
    /////     Describe the format of the corresponding version of JSON output by writing to standard output a JSON
    /////     object with the same structure as the JSON generated by qpdf. In the output written by <see cref="JsonHelp"/>,
    /////     each key’s value is a description of the key. The specific contract guaranteed by qpdf in its JSON
    /////     representation is explained in more detail in the qpdf JSON. The default version of help is version 2,
    /////     as with the <see cref="Json"/> flag.
    ///// </summary>
    ///// <param name="version"><see cref="JsonVersion"/></param>
    ///// <returns>
    /////     <see cref="Job" />
    ///// </returns>
    //public Job JsonHelp(JsonVersion version = JsonVersion.Version2)
    //{
    //    Logger.LogInformation($"Generating JSON {version} help output");
    //    _jsonHelp = version.ToString();
    //    return this;
    //}
    //#endregion

    #region JsonKey
    /// <summary>
    ///     This option is repeatable. If given, only the specified top-level keys will be included in the JSON output.
    ///     Otherwise, all keys will be included. <b>version</b> and <b>parameters</b> will always appear in the output.
    /// </summary>
    /// <param name="key">The key to show</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job JsonKey(string key)
    {
        _jsonKey ??= new List<string>();
        _jsonKey.Add(key);
        Logger.LogInformation($"Only include top-level key '{key}' in JSON output");
        return this;
    }
    #endregion

    #region JsonObject
    /// <summary>
    ///     This option is repeatable. If given, only specified objects will be shown in the “objects” key of the JSON output.
    ///     Otherwise, all objects will be shown.
    /// </summary>
    /// <param name="obj">The object to only include</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job JsonObject(string obj)
    {
        _jsonObject ??= new List<string>();
        _jsonObject.Add(obj);
        Logger.LogInformation($"Only include object '{obj}' in JSON output");
        return this;
    }
    #endregion

    #region JsonStreamData
    /// <summary>
    ///     When used with <see cref="JsonStreamData.File"/>, <see cref="JsonStreamPrefix"/> sets the prefix
    ///     for stream data files, overriding the default, which is to use the output file name. Whatever is
    ///     given here will be appended with -nnn to create the name of the file that will contain the data
    ///     for the stream in object nnn.
    /// </summary>
    /// <param name="streamData"><see cref="StreamData"/></param>
    /// <remarks>
    ///     Use <see cref="Run(out string?, out byte[])"/> instead of <see cref="Run(out string?)"/> when using this method 
    /// </remarks>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job JsonStreamData(JsonStreamData streamData)
    {
        Logger.LogInformation($"Setting JSON stream data to '{streamData}'");

        switch (streamData)
        {
            case Enums.JsonStreamData.None:
                _jsonStreamData = string.Empty;
                break;
            case Enums.JsonStreamData.Inline:
                _jsonStreamData = "inline";
                break;
            case Enums.JsonStreamData.File:
                _jsonStreamData = "file";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(streamData), streamData, null);
        }
        return this;
    }
    #endregion
     
    #region JsonStreamPrefix
    /// <summary>
    ///     When used with <see cref="JsonStreamData.File"/>, <see cref="JsonStreamPrefix"/> sets the prefix for
    ///     stream data files, overriding the default, which is to use the output file name. Whatever is given
    ///     here will be appended with -nnn to create the name of the file that will contain the data for the
    ///     stream in object nnn.
    /// </summary>
    /// <param name="prefix">The file prefix</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job JsonStreamPrefix(string prefix)
    {
        Logger.LogInformation($"Setting JSON stream prefix to '{prefix}'");
        _jsonStreamPrefix = prefix;
        return this;
    }
    #endregion

    #region JsonOutput
    /// <summary>
    ///     Implies <see cref="Json"/> at the specified version. This option changes several default values, all of
    ///     which can be overridden by specifying the stated option:<br/>
    ///     - The default value for <see cref="JsonStreamData"/> changes from none to inline.<br/>
    ///     - The default value for <see cref="DecodeLevel"/> changes from generalized to none.<br/>
    ///     - By default, only the "qpdf" key is included in the JSON output, but you can add additional keys with <see cref="JsonKey"/>.
    ///     - The "version" and "parameters" keys will be excluded from the JSON output.
    ///
    ///     If you want to look at the contents of streams easily as you would in QDF mode (see QDF Mode), you can use
    ///     <see cref="DecodeLevel"/> generalized and <see cref="JsonStreamData"/> file for a convenient way to do that.
    /// </summary>
    /// <param name="version"><see cref="JsonVersion"/></param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job JsonOutput(JsonVersion version = JsonVersion.Version2)
    {
        Logger.LogInformation($"Setting JSON output format to version {version}");
        _jsonOutput = ((int)version).ToString();
        return this;
    }
    #endregion

    #region JsonInput
    /// <summary>
    ///     Treat the input file as a JSON file in qpdf JSON format. The input file must be complete and include all stream data.
    ///     The JSON version must be at least 2. All top-level keys are ignored except for "qpdf". For information about converting
    ///     between PDF and JSON, please see qpdf JSON.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job JsonInput()
    {
        Logger.LogInformation("Treat the input file as a JSON file in qpdf JSON format.");
        _jsonInput = string.Empty;
        return this;
    }
    #endregion

    #region UpdateFromJson
    /// <summary>
    ///     Treat the input file as a JSON file in qpdf JSON format. The input file must be complete and include all stream data.
    ///     The JSON version must be at least 2. All top-level keys are ignored except for "qpdf". For information about converting
    ///     between PDF and JSON, please see qpdf JSON.
    /// </summary>
    /// <param name="jsonFile">an input JSON file in qpdf format</param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job UpdateFromJson(string jsonFile)
    {
        Logger.LogInformation($"Updating PDF with qpdf JSON input file '{jsonFile}'");
        _updateFromJson = jsonFile;
        return this;
    }
    #endregion

    #region StaticId
    /// <summary>
    ///     Use a fixed value for the document ID (/ID in the trailer). This is intended for testing only. <b>Never</b> use it for
    ///     production files. If you are trying to get the same ID each time for a given file, and you are not generating encrypted files,
    ///     consider using the <see cref="DeterministicId"/> option.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job StaticId()
    {
        Logger.LogInformation("Using a fixed value for the document ID (/ID in the trailer)");
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
        Logger.LogInformation("Using a static initialization vector for AES-CBC");
        _staticAesIv = string.Empty;
        return this;
    }
    #endregion

    #region LinearizePass1
    /// <summary>
    ///     Write the first pass of linearization to the named file. The resulting file is not a valid PDF file. This option is useful
    ///     only for debugging QPDFWriter’s linearization code. When qpdf linearizes files, it writes the file in two passes, using the
    ///     first pass to calculate sizes and offsets that are required for hint tables and the linearization dictionary. Ordinarily, the
    ///     first pass is discarded. This option enables it to be captured, allowing inspection of the file before values calculated in
    ///     pass 1 are inserted into the file for pass 2.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job LinearizePass1(string fileName)
    {
        Logger.LogInformation("Writing the first pass of linearization to the named file. The resulting file is not a valid PDF file");
        _linearizePass1 = fileName;
        return this;
    }
    #endregion

    #region InternalRun
    /// <summary>
    ///     Runs the job with the given parameters
    /// </summary>
    /// <param name="output">Returns any output that is generated by qpdf</param>
    /// <param name="data">Only some methods return data</param>
    private int InternalRun(out string? output, out byte[]? data)
    {
        data = null;

        var settings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        settings.Converters.Add(new StringEnumConverter());

        var json = JsonConvert.SerializeObject(this, settings);

        Logger.LogDebug($"JSON input for QPDF: {Environment.NewLine}{json}");

        _info.Clear();
        _warn.Clear();
        _error.Clear();
        _output.Clear();
        _save.Clear();

        var jobHandle = Native.qpdfjob_init();
        Native.qpdfjob_set_logger(jobHandle: jobHandle, loggerHandle: _loggerHandle);
        Native.qpdfjob_initialize_from_json(jobHandle, json);

        var result = Native.qpdfjob_run(jobHandle);

        _output.Append(_info);
        _output.Append(_warn);
        _output.Append(_error);

        output = _output.ToString().TrimEnd();

        if (_save.Count > 0)
            data = _save.ToArray();

        Logger.LogInformation("Output from QPDF: " + Environment.NewLine + output);

        Native.qpdfjob_cleanup(jobHandle);

        Reset();

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
            throw new ArgumentException("Use the method 'RunRequiresPassword' when the RequiresPassword method is used");

        return (ExitCode)InternalRun(out output, out _);
    }

    /// <summary>
    ///     Runs the <see cref="Job" /> with the given parameters
    /// </summary>
    /// <param name="output">Returns any output that is generated by qpdf</param>
    /// <param name="data">Returns the data or <c>null</c> when something goes wrong</param>
    /// <returns>
    ///     <see cref="ExitCode" />
    /// </returns>
    public ExitCode Run(out string? output, out byte[]? data)
    {
        if (_isEncrypted == string.Empty)
            throw new ArgumentException("Use the method 'RunIsEncrypted' when the IsEncrypted method is used");

        if (_requiresPassword == string.Empty)
            throw new ArgumentException("Use the method 'RunRequiresPassword' when the RequiresPassword method is used");

        return (ExitCode)InternalRun(out output, out data);
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

        return (ExitCodeIsEncrypted)InternalRun(out output, out _);
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

        return (ExitCodeRequiresPassword)InternalRun(out output, out _);
    }
    #endregion

    #region Dispose
    /// <summary>
    ///     Disposes this object
    /// </summary>
    public void Dispose()
    {
        _infoHandle.Free();
        _warnHandle.Free();
        _errorHandle.Free();
        _saveHandle.Free();

        Native.qpdflogger_cleanup(_loggerHandle);
    }
    #endregion
}
