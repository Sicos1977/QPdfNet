using System.Runtime.Serialization;

namespace QPdfNet.Enums
{
    /// <summary>
    ///     Modify options
    /// </summary>
    public enum EncryptionModify
    {
        /// <summary>
        ///     No options haven been set
        /// </summary>
        NotSet,

        /// <summary>
        ///     No modifications allowed
        /// </summary>
        [EnumMember(Value = "none")]
        None,

        /// <summary>
        ///     Assembly allowed
        /// </summary>
        [EnumMember(Value = "assembly")]
        Assembly,

        /// <summary>
        ///     Form filling allowed
        /// </summary>
        [EnumMember(Value = "form")]
        Form,

        /// <summary>
        ///     Annotating allowed
        /// </summary>
        [EnumMember(Value = "Annotate")]
        Annotate
    }
}
