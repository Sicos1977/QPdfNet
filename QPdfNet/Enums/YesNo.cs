using System.Runtime.Serialization;

namespace QPdfNet.Enums
{
    /// <summary>
    ///     Yes or no
    /// </summary>
    public enum YesNo
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
