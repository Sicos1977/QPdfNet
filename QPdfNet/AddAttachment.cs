using System;
using System.IO;
using System.Text.Json.Serialization;
using QPdfNet.Helpers;

namespace QPdfNet;

/// <summary>
///     Adding attachment
/// </summary>
internal class AddAttachment
{
    #region Properties
    [JsonPropertyName("key")] 
    public string Key { get; private set; }
    
    [JsonPropertyName("file")] 
    public string File { get; private set; }
    
    [JsonPropertyName("filename")] 
    public string FileName { get; private set; }
    
    [JsonPropertyName("creationdate")] 
    public string CreationDate { get; private set; }

    [JsonPropertyName("moddate")] 
    public string ModDate { get; private set; }

    [JsonPropertyName("mimetype")] 
    public string MimeType { get; private set; }

    [JsonPropertyName("description")] 
    public string? Description { get; private set; }

    [JsonPropertyName("replace")] 
    public string? Replace { get; private set; }
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
    public AddAttachment(string fileName, string? description = null, bool replace = false)
    {
        var file = new FileInfo(fileName);

        if (!file.Exists)
            throw new FileNotFoundException($"The file '{fileName}' could not be found");

        File = file.FullName;
        Key = Path.GetFileName(fileName);
        FileName = file.Name;
        CreationDate = $"D:{file.CreationTimeUtc:yyyyMMddHHmmsss}Z";
        ModDate = $"D:{file.CreationTimeUtc:yyyyMMddHHmmsss}Z";
        MimeType = MimeTypes.GetMimeType(fileName);
        Description = description;

        if (replace)
            Replace = string.Empty;
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
    /// <exception cref="FileNotFoundException"></exception>
    public AddAttachment(
        string file,
        string key,
        string fileName,
        DateTime creationDate,
        DateTime modData,
        string mimeType,
        string? description = null,
        bool replace = false)
    {
        if (!System.IO.File.Exists(fileName))
            throw new FileNotFoundException($"The file '{fileName}' could not be found");

        File = file;
        Key = key;
        FileName = fileName;
        CreationDate = $"D:{creationDate.ToUniversalTime():yyyyMMddHHmmsss}Z";
        ModDate = $"D:{modData.ToUniversalTime():yyyyMMddHHmmss}Z";
        MimeType = mimeType;
        Description = description;

        if (replace)
            Replace = string.Empty;
    }
    #endregion
}