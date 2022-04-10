using System.Runtime.Serialization;

namespace QPdfNet.Enums
{
    /// <summary>
    ///     Compress streams
    /// </summary>
    public enum CompressStreams
    {
        /// <summary>
        ///     Yes
        /// </summary>
        [EnumMember(Value = "y")]
        Yes,

        /// <summary>
        ///     no
        /// </summary>
        [EnumMember(Value = "n")]
        No,
    }
}
