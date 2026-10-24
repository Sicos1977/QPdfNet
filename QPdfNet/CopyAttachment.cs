using System.IO;
using System.Text.Json.Serialization;

namespace QPdfNet;

/// <summary>
///     Copying attachment
/// </summary>
internal class CopyAttachment
{
    #region Properties
    [JsonPropertyName("file")]
    public string File { get; private set; }

    [JsonPropertyName("prefix")]
    public string? Prefix { get; private set; }
    #endregion

    #region Constructor
    /// <summary>
    ///     Makes this object and sets all it's needed properties
    /// </summary>
    /// <param name="file">The file to copy from with its full path</param>
    /// <param name="prefix">
    ///     Only required if the file from which attachments are being copied has attachments with keys that
    ///     conflict with attachments already in the file. In this case, the specified prefix will be prepended to each key.
    ///     This affects only the key in the embedded files table, not the file name. The PDF specification doesn’t preclude
    ///     multiple attachments having the same file name.
    /// </param>
    /// <exception cref="FileNotFoundException"></exception>
    public CopyAttachment(string file, string? prefix = null)
    {
        File = file;
        Prefix = prefix;
    }
    #endregion
}