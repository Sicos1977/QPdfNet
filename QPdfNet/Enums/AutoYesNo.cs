using System.Runtime.Serialization;

namespace QPdfNet.Enums
{
    /// <summary>
    ///     Auto, yes or no
    /// </summary>
    public enum AutoYesNo
    {
        /// <summary>
        ///     Auto
        /// </summary>
        [EnumMember(Value = "auto")]
        Auto,

        /// <summary>
        ///     Yes
        /// </summary>
        [EnumMember(Value = "y")]
        Yes,

        /// <summary>
        ///     no
        /// </summary>
        [EnumMember(Value = "n")]
        No
    }
}
