using System.Runtime.Serialization;

namespace QPdfNet.Enums
{
    /// <summary>
    ///     Flatten Annotation Parameters
    /// </summary>
    public enum FlattenAnnotations
    {
        /// <summary>
        ///     Include all annotations that are not marked invisible or hidden
        /// </summary>
        [EnumMember(Value = "all")]
        All,

        /// <summary>
        ///     Only include annotations that should appear when the page is printed
        /// </summary>
        [EnumMember(Value = "print")]
        Print,

        /// <summary>
        ///     Omit annotations that should not appear on the screen
        /// </summary>
        [EnumMember(Value = "screen")]
        Screen
    }
}
