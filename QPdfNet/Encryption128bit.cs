using QPdfNet.Enums;

namespace QPdfNet
{
    /// <summary>
    ///     Use 128 bit encryption
    /// </summary>
    public class Encryption128Bit : Encryption256Bit
    {
        /// <summary>
        ///      Encrypt the PDF with 128 bit encryption
        /// </summary>
        /// <param name="accessibility">Restrict accessibility (usually ignored)</param>
        /// <param name="annotate">Restrict commenting/filling form fields</param>
        /// <param name="assemble">Restrict document assembly</param>
        /// <param name="extract">Restrict text/graphic extraction</param>
        /// <param name="form">Restrict filling form fields</param>
        /// <param name="modifyOther">Restrict other modifications</param>
        /// <param name="modify">Control <see cref="Enums.Modify"/> access by level</param>
        /// <param name="print">Control printing access</param>
        /// <param name="cleartextMetaData">Prevent encryption of metadata</param>

        public Encryption128Bit(
            bool accessibility = true,
            bool annotate = true,
            bool assemble = true,
            bool extract = true,
            bool form = true,
            bool modifyOther = true,
            Modify modify = Modify.All,
            Print print = Print.Full) :
            base(
                accessibility,
                annotate,
                assemble,
                extract,
                form,
                modifyOther,
                modify,
                print)
        {

        }
    }
}
