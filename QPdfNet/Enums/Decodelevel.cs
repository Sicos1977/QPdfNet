using System.Runtime.Serialization;

namespace QPdfNet.Enums;

/// <summary>
///     Decode level
/// </summary>
public enum DecodeLevel
{
    /// <summary>
    ///     Do not attempt to decode any streams
    /// </summary>
    [EnumMember(Value = "none")] 
    None,

    /// <summary>
    ///     Decode streams filtered with supported generalized filters: /LZWDecode, /FlateDecode, /ASCII85Decode, and
    ///     /ASCIIHexDecode
    /// </summary>
    [EnumMember(Value = "generalized")] 
    Generalized,

    /// <summary>
    ///     In addition to generalized, decode streams with supported non-lossy specialized filters; currently this is just
    ///     /RunLengthDecode
    /// </summary>
    [EnumMember(Value = "specialized")] 
    Specialized,

    /// <summary>
    ///     In addition to generalized and specialized, decode streams with supported lossy filters; currently this is just
    ///     /DCTDecode (JPEG)
    /// </summary>
    [EnumMember(Value = "all")] 
    All
}