using System.Runtime.Serialization;

namespace QPdfNet.Enums
{
    /// <summary>
    ///     Controls handling of object streams
    /// </summary>
    public enum ObjectStreams
    {
        /// <summary>
        ///     Preserve original object streams, if any (the default)
        /// </summary>
        [EnumMember(Value = "preserve")]
        Preserve,

        /// <summary>
        ///     Create output files with no object streams
        /// </summary>
        [EnumMember(Value = "disable")]
        Disable,

        /// <summary>
        ///     Create object streams, and compress objects when possible
        /// </summary>
        [EnumMember(Value = "generate")]
        Generate
    }
}
