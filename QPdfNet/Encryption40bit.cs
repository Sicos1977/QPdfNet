using Newtonsoft.Json;
using QPdfNet.Enums;
using QPdfNet.Interfaces;

namespace QPdfNet
{
    /// <summary>
    ///     Use 40 bit encryption
    /// </summary>
    public class Encryption40Bit : IEncryption
    {
        #region Fields
        [JsonProperty("annotate")]
        private string _annotate;

        [JsonProperty("extract")]
        private string _extract;

        [JsonProperty("modify")]
        private Modify _modify;

        [JsonProperty("print")]
        private Print _print;
        #endregion

        #region Constructor
        /// <summary>
        ///     Encrypt the PDF with 40 bit encryption
        /// </summary>
        /// <param name="annotate">Restrict comments, filling forms, and signing</param>
        /// <param name="extract">Restrict text/graphic extraction</param>
        /// <param name="modify">Restrict document <see cref="Modify"/></param>
        /// <param name="print">Restrict <see cref="Print"/></param>
        public Encryption40Bit(
            bool annotate = true, 
            bool extract = true, 
            Modify modify = Modify.All, 
            Print print = Print.Full)
        {
            _annotate = annotate ? "y" : "n";
            _extract = extract ? "y" : "n";
            _modify = modify;
            _print = print;
        }
        #endregion
    }
}
