
using System.Diagnostics;
using DIaLOGIKa.b2xtranslator.StructuredStorage.Reader;

namespace DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.Records
{
    [BiffRecord(RecordType.VCenter)] 
    public class VCenter : BiffRecord
    {
        public const RecordType ID = RecordType.VCenter;

        public bool vcenter; 

        public VCenter(IStreamReader reader, RecordType id, ushort length)
            : base(reader, id, length)
        {
            // assert that the correct record type is instantiated
            Debug.Assert(this.Id == ID);

            var buffer = this.Reader.ReadUInt16();
            if (buffer == 1)
            {
                this.vcenter = true;
            }
            else
            {
                this.vcenter = false;
            }
            

        }
    }
}
