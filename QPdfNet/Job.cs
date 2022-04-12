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
using System.IO;
using System.Text;
using Newtonsoft.Json;
using QPdfNet.Enums;
using QPdfNet.Interfaces;
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
    public Job Encrypt(string userPassword, string ownerPassword, IEncryption options)
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
    ///     <see cref="EncryptionFilePassword"/> option to set it. Note that copying the encryption parameters from a file also copies
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
    ///     By default, or with <see cref="CompressStreams" /> = <see cref="YesNo.Yes" />, qpdf will compress
    ///     streams using the flate compression algorithm (used by zip and gzip) unless those streams are compressed in some
    ///     other way. This analysis is made after qpdf attempts to uncompress streams and is therefore closely related to
    ///     <see cref="DecodeLevel" />. To suppress this behavior and leave streams streams uncompressed, use
    ///     <see cref="CompressStreams" /> = <see cref="YesNo.No" />. In QDF
    ///     mode (see QDF Mode and <see cref="QPdf"/>), the default is to leave streams uncompressed.
    /// </summary>
    /// <param name="compress"><c>true</c> or <c>false</c></param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job CompressStreams(bool compress)
    {
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
        _decodeLevel = decodeLevel;
        return this;
    }
    #endregion

    #region StreamData
    /// <summary>
    ///     Controls transformation of stream data. This option predates the <see cref="CompressStreams" /> and
    ///     <see cref="DecodeLevel" /> options. Those options can be used to achieve the same effect with more control
    /// </summary>
    /// <param name="streamData"></param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job StreamData(StreamData streamData = Enums.StreamData.Compress)
    {
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
        _recompressFlate = string.Empty;
        return this;
    }
    #endregion

    #region CompressionLevel
    /// <summary>
    ///     When writing new streams that are compressed with /FlateDecode, use the specified compression
    ///     <paramref name="level" />. The value of <paramref name="level" /> should be a number from 1 to 9 and is passed
    ///     directly to zlib, which implements deflate
    ///     compression. Lower numbers compress less and are faster; higher numbers compress more and are slower. Note that
    ///     qpdf doesn’t
    ///     uncompress and recompress streams compressed with flate by default. To have this option apply to already compressed
    ///     streams, you should also specify <see cref="RecompressFlate" />. If your goal is to shrink the size of PDF files,
    ///     you should also use <see cref="ObjectStreams" />. If you omit this option, qpdf defers to the compression library’s
    ///     default behavior.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job CompressionLevel(int level)
    {
        if (level is < 1 or > 9)
            throw new ArgumentOutOfRangeException(nameof(level), "Should be in the range 1 to 9");

        _compressionLevel = level.ToString();
        return this;
    }
    #endregion

    #region NormalizeContent
    /// <summary>
    ///     Enables or disables normalization of newlines in PDF content streams to UNIX-style newlines, which is useful for
    ///     viewing files in a programmer-friendly text edit across multiple platforms. Content normalization is off by
    ///     default, but is automatically enabled by <see cref="QPdf" /> (see also QDF Mode). It is not recommended to use this
    ///     option for production use. If qpdf runs into any lexical errors while normalizing content, it will print a warning
    ///     indicating
    ///     that content may be damaged.
    /// </summary>
    /// <param name="normalize"><c>true</c> or <c>false</c></param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job NormalizeContent(bool normalize)
    {
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
    ///     <see cref="ObjectStreams" />
    /// </param>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job ObjectStreams(ObjectStreams objectStreams)
    {
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
    ///     <see cref="RemoveUnreferencedResources" />.
    ///     See also <see cref="PreserveUnreferenced" />, which does something completely different. To reduce confusion, you
    ///     should use <see cref="RemoveUnreferencedResources" /> = <see cref="AutoYesNo.No" /> instead.
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

    #region NewlineBeforeEndStream
    /// <summary>
    ///     Tell qpdf to insert a newline before the endstream keyword, not counted in the length, after any stream content
    ///     even if the last character of the stream was a newline. This may result in two newlines in some cases. This is a
    ///     requirement of PDF/A. While qpdf doesn’t specifically know how to generate PDF/A-compliant PDFs, this at least
    ///     prevents it from removing compliance on already compliant files.
    /// </summary>
    /// <returns>
    ///     <see cref="Job" />
    /// </returns>
    public Job NewlineBeforeEndStream()
    {
        _newlineBeforeEndStream = string.Empty;
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
    ///     this option is selected.
    ///     Use <see cref="IiMinBytes" /> to change the size threshold. This option is implicitly selected when
    ///     <see cref="OptimizeImages" /> is selected unless <see cref="KeepInlineImages" /> is also specified.
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
    ///     1,024 bytes. Use 0 for no minimum.
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
    ///     increase the version as needed when adding features that require newer PDF readers.
    ///     The version number may be expressed in the form major.minor[.extension-level]. If .extension-level, is given,
    ///     version is interpreted as major.minor at extension level extension-level. For example, version 1.7.8 represents
    ///     version 1.7 at extension level 8. Note that minimal syntax checking is done on the command line. qpdf does not
    ///     check whether the specified version is actually required.
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
    ///     not supported in that version. The version number is interpreted in the same way as with --min-version so that
    ///     extension levels can be set. In some cases, forcing the output file’s PDF version to be lower than that of the
    ///     input file will cause qpdf to disable certain features of the document. Specifically, 256-bit keys are disabled if
    ///     the version is less than 1.7 with extension level 8 (except the deprecated, unsupported “R5” format is allowed with
    ///     extension levels 3 through 7), AES encryption is disabled if the version is less than 1.6, clear text metadata and
    ///     object streams are disabled if less than 1.5, 128-bit encryption keys are disabled if less than 1.4, and all
    ///     encryption is disabled if less than 1.3. Even with these precautions, qpdf won’t be able to do things like
    ///     eliminate use of newer image compression schemes, transparency groups, or other features that may have been added
    ///     in more recent versions of PDF.
    ///     As a general rule, with the exception of big structural things like the use of object streams or AES encryption,
    ///     PDF viewers are supposed to ignore features they don’t support. This means that forcing the version to a lower
    ///     version may make it possible to open your PDF file with an older version, though bear in mind that some of the
    ///     original document’s functionality may be lost.
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

    #region Collate
    /// <summary>
    ///     This option causes qpdf to collate rather than concatenate pages specified with --pages. With a numeric parameter,
    ///     collate in groups of <paramref name="n" />. The default is 1.
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

    // TODO: https://qpdf.readthedocs.io/en/stable/cli.html?highlight=ranges#option-pages

    #region SplitPages
    /// <summary>
    ///     Write each group of <paramref name="n" /> pages to a separate  <see cref="OutputFile" />. If <paramref name="n" />
    ///     is not specified, create single pages. Output file names are generated as follows:
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

    // TODO: https://qpdf.readthedocs.io/en/stable/cli.html?highlight=ranges#option-overlay

    // TODO: https://qpdf.readthedocs.io/en/stable/cli.html?highlight=ranges#option-underlay

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
    ///     The page range or <c>null</c>, see
    ///     https://qpdf.readthedocs.io/en/stable/cli.html?highlight=ranges#page-ranges
    /// </param>
    /// <returns></returns>
    public Job Rotate(Rotation angle, string pageRange = null)
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
    ///     <see cref="OiMinWidth" />,
    ///     <see cref="OiMinHeight" />, and <see cref="OiMinArea" /> options. By default, inline images are converted to
    ///     regular images and optimized
    ///     as well. Use --keep-inline-images to prevent inline images from being included.
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
    ///     Avoid optimizing images whose width is below the specified amount. If omitted, the default is 128 pixels. Use 0 for
    ///     no minimum.
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
    ///     Avoid optimizing images whose height is below the specified amount. If omitted, the default is 128 pixels. Use 0
    ///     for no minimum.
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
    ///     is 16,384 pixels. Use 0 for no minimum.
    /// </summary>
    /// <param name="pixels">Pixels</param>
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
    ///    Prevent inline images from being included in image optimization done by <see cref="OptimizeImages"/>.
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
    ///    Exclude page labels (explicit page numbers) from the output file.
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

    #region Run
    /// <summary>
    ///     Runs the job with the given parameters
    /// </summary>
    /// <returns>
    ///     <see cref="ExitCodes" />
    /// </returns>
    public ExitCodes Run()
    {
        var settings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore, Formatting = Formatting.Indented };
        settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        var json = JsonConvert.SerializeObject(this, settings);

        using var memoryStream = new MemoryStream();
        Console.SetError(new StreamWriter(memoryStream));
        Console.SetOut(new StreamWriter(memoryStream));
        var t = Console.IsErrorRedirected;
        var tt = Console.IsOutputRedirected;

        // https://code-examples.net/en/q/ee192e

        var result = QPdfApi.Native.RunFromJSON(json);
        var test = Encoding.UTF8.GetString(memoryStream.ToArray());
        return result;
    }
    #endregion

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
    [JsonProperty("compressStreams")] private string _compressStreams;
    [JsonProperty("decodeLevel")] private DecodeLevel _decodeLevel;
    [JsonProperty("streamData")] private StreamData _streamData;
    [JsonProperty("recompressFlate")] private string _recompressFlate;
    [JsonProperty("compressionLevel")] private string _compressionLevel;
    [JsonProperty("normalizeContent")] private string _normalizeContent;
    [JsonProperty("objectStreams")] private ObjectStreams _objectStreams;
    [JsonProperty("preserveUnreferenced")] private string _preserveUnreferenced;

    [JsonProperty("removeUnreferencedResources")]
    private AutoYesNo _removeUnreferencedResources;

    [JsonProperty("preserveUnreferencedResources")]
    private string _preserveUnreferencedResources;

    [JsonProperty("newlineBeforeEndStream")]
    private string _newlineBeforeEndStream;

    [JsonProperty("coalesceContents")] private string _coalesceContents;

    [JsonProperty("externalizeInlineImages")]
    private string _externalizeInlineImages;

    [JsonProperty("iiMinBytes")] private string _iiMinBytes;
    [JsonProperty("minVersion")] private string _minVersion;
    [JsonProperty("forceVersion")] private string _forceVersion;
    [JsonProperty("collate")] private string _collate;
    [JsonProperty("splitPages")] private string _splitPages;
    [JsonProperty("flattenRotation")] private string _flattenRotation;
    [JsonProperty("flattenAnnotations")] private FlattenAnnotations _flattenAnnotation;
    [JsonProperty("rotate")] private string _rotate;
    [JsonProperty("generateAppearances")] private string _generateAppearances;
    [JsonProperty("optimizeImages")] private string _optimizeImages;
    [JsonProperty("oiMinWidth")] private string _oiMinWidth;
    [JsonProperty("oiMinHeight")] private string _oiMinHeight;
    [JsonProperty("oiMinArea")] private string _oiMinArea;
    [JsonProperty("keepInlineImages")] private string _keepInlineImages;
    [JsonProperty("removePageLabels")] private string _removePageLabels;
    #endregion
}