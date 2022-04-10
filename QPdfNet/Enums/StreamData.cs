using System.Runtime.Serialization;

namespace QPdfNet.Enums
{
    /// <summary>
    ///     Controls transformation of stream data
    /// </summary>
    public enum StreamData
    {
        /// <summary>
        ///     Recompress stream data when possible (default)
        /// </summary>
        [EnumMember(Value = "compress")]
        Compress,

        /// <summary>
        ///     Leave all stream data as is
        /// </summary>
        [EnumMember(Value = "preserve")]
        Preserve,

        /// <summary>
        ///     Uncompress stream data compressed with generalized filters when possible
        /// </summary>
        [EnumMember(Value = "uncompress")]
        Uncompress
    }
}
