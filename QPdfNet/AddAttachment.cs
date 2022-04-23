using System;
using System.IO;
using Newtonsoft.Json;
using QPdfNet.Helpers;

namespace QPdfNet;

/// <summary>
///     Adding attachment
/// </summary>
internal class AddAttachment
{
    #region Fields
    [JsonProperty("key")] private string _key;
    [JsonProperty("file")] private string _file;
    [JsonProperty("filename")] private string _filename;
    [JsonProperty("creationdate")] private string _creationdate;
    [JsonProperty("moddate")] private string _moddate;
    [JsonProperty("mimetype")] private string _mimetype;
    [JsonProperty("description")] private string _description;
    [JsonProperty("replace")] private string _replace;
    #endregion

    #region Constructor
    /// <summary>
    ///     Makes this object and sets all it's needed properties
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
    /// <exception cref="FileNotFoundException"></exception>
    public AddAttachment(string fileName, string description = null, bool replace = false)
    {
        var file = new FileInfo(fileName);

        if (!file.Exists)
            throw new FileNotFoundException($"The file '{fileName}' could not be found");

        _file = file.FullName;
        _key = Path.GetFileName(fileName);
        _filename = file.Name;
        _creationdate = $"D:{file.CreationTimeUtc:yyyyMMddHHmmsss}Z";
        _moddate = $"D:{file.CreationTimeUtc:yyyyMMddHHmmsss}Z";
        _mimetype = MimeTypes.GetMimeType(fileName);
        _description = description;

        if (replace)
            _replace = string.Empty;
    }

    /// <summary>
    ///     Makes this object and sets all it's needed properties
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
    /// <param name="prefix">
    ///     Only required if the file from which attachments are being copied has attachments with keys that
    ///     conflict with attachments already in the file. In this case, the specified prefix will be prepended to each key.
    ///     This affects only the key in the embedded files table, not the file name. The PDF specification doesn’t preclude
    ///     multiple attachments having the same file name.
    /// </param>
    /// <exception cref="FileNotFoundException"></exception>
    public AddAttachment(
        string file,
        string key,
        string fileName,
        DateTime creationDate,
        DateTime modData,
        string mimeType,
        string description = null,
        bool replace = false)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"The file '{fileName}' could not be found");

        _file = file;
        _key = key;
        _filename = fileName;
        _creationdate = $"D:{creationDate.ToUniversalTime():yyyyMMddHHmmsss}Z";
        _moddate = $"D:{modData.ToUniversalTime():yyyyMMddHHmmss}Z";
        _mimetype = mimeType;
        _description = description;

        if (replace)
            _replace = string.Empty;
    }
    #endregion
}