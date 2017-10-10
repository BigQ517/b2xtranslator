
using System.Diagnostics;
using DIaLOGIKa.b2xtranslator.StructuredStorage.Reader;

namespace DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.Records
{
    [BiffRecord(RecordType.HCenter)] 
    public class HCenter : BiffRecord
    {
        public const RecordType ID = RecordType.HCenter;

        public bool hcenter; 

        public HCenter(IStreamReader reader, RecordType id, ushort length)
            : base(reader, id, length)
        {
            // assert that the correct record type is instantiated
            Debug.Assert(this.Id == ID);

            var buffer = this.Reader.ReadUInt16();
            if (buffer == 1)
            {
                this.hcenter = true;
            }
            else
            {
                this.hcenter = false; 
            }

        }
    }
}
