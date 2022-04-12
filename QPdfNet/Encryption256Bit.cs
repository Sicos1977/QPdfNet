using Newtonsoft.Json;
using QPdfNet.Enums;
using QPdfNet.Interfaces;

namespace QPdfNet
{
    /// <summary>
    ///     Use 256 bit encryption
    /// </summary>
    public class Encryption256Bit : IEncryption
    {
        #region Fields
        [JsonProperty("accessibility")]
        private string _accessibility;

        [JsonProperty("annotate")]
        private string _annotate;

        [JsonProperty("assemble")]
        private string _assemble;

        [JsonProperty("extract")]
        private string _extract;

        [JsonProperty("form")]
        private string _form;

        [JsonProperty("modifyOther")]
        private string _modifyOther;
        
        [JsonProperty("modify")]
        private Modify _modify;
        
        [JsonProperty("print")]
        private Print _print;

        [JsonProperty("cleartextMetadata")]
        private string _cleartextMetadata;
        #endregion

        #region Constructor
        /// <summary>
        ///      Encrypt the PDF with 256 bit encryption
        /// </summary>
        /// <param name="accessibility">Restrict accessibility (usually ignored)</param>
        /// <param name="annotate">Restrict commenting/filling form fields</param>
        /// <param name="assemble">Restrict document assembly</param>
        /// <param name="extract">Restrict text/graphic extraction</param>
        /// <param name="form">Restrict filling form fields</param>
        /// <param name="modifyOther">Restrict other modifications</param>
        /// <param name="modify">Control <see cref="_modify"/> access by level</param>
        /// <param name="print">Control printing access</param>
        /// <param name="cleartextMetaData">Prevent encryption of metadata</param>
        public Encryption256Bit(
            bool accessibility = true, 
            bool annotate = true, 
            bool assemble = true, 
            bool extract = true, 
            bool form = true, 
            bool modifyOther = true,
            Modify modify = Modify.All,
            Print print = Print.Full,
            bool cleartextMetaData = false)
        {
            _accessibility = accessibility ? "y" : "n";
            _annotate = annotate ? "y" : "n";
            _assemble = assemble ? "y" : "n";
            _extract = extract ? "y" : "n";
            _form = form ? "y" : "n";
            _modifyOther = modifyOther ? "y" : "n";
            _modify = modify;
            _print = print;

            //if (cleartextMetaData)
                _cleartextMetadata = "y";
        }
    }
    #endregion
}
