using System.Diagnostics;
using DIaLOGIKa.b2xtranslator.StructuredStorage.Reader;

namespace DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.Records
{
    [BiffRecord(RecordType.BottomMargin)]
    public class BottomMargin : BiffRecord
    {
        public const RecordType ID = RecordType.BottomMargin;
        public double value; 

        public BottomMargin(IStreamReader reader, RecordType id, ushort length)
            : base(reader, id, length)
        {
            // assert that the correct record type is instantiated
            Debug.Assert(this.Id == ID);

            this.value = reader.ReadDouble(); 
        }
    }
}
