using System.Diagnostics;
using DIaLOGIKa.b2xtranslator.StructuredStorage.Reader;

namespace DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.Ptg
{
    public class PtgPower : AbstractPtg
    {
        public const PtgNumber ID = PtgNumber.PtgPower;

        public PtgPower(IStreamReader reader, PtgNumber ptgid)
            :
            base(reader, ptgid)
        {
            Debug.Assert(this.Id == ID);
            this.Length = 1;
            this.Data = "^";
            this.type = PtgType.Operator;
            this.popSize = 2;
        }
    }
}
