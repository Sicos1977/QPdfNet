using System.Runtime.Serialization;

namespace QPdfNet.Enums
{
    /// <summary>
    ///     Printing options
    /// </summary>
    public enum EncryptionPrint
    {
        /// <summary>
        ///     No options haven been set
        /// </summary>
        NotSet,

        /// <summary>
        ///     No printing
        /// </summary>
        [EnumMember(Value = "none")]
        None,

        /// <summary>
        ///     Only printing in low quality
        /// </summary>
        [EnumMember(Value = "low")]
        Low
    }
}
