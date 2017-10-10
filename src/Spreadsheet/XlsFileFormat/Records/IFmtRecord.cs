﻿

using System.Diagnostics;
using DIaLOGIKa.b2xtranslator.StructuredStorage.Reader;

namespace DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.Records
{
    /// <summary>
    /// This record specifies the number format to use for the text on an axis.
    /// </summary>
    [BiffRecord(RecordType.IFmtRecord)]
    public class IFmtRecord : BiffRecord
    {
        public const RecordType ID = RecordType.IFmtRecord;

        /// <summary>
        /// An unsigned integer that specifies the identifier of a number format. 
        /// The identifier specified by this field MUST be a valid built-in number
        /// format identifier or the identifier of a custom number format as specified 
        /// using a Format record. Custom number format identifiers MUST be greater than 
        /// or equal to 0x00A4 less than or equal to 0x0188, and SHOULD <78> be less 
        /// than or equal to 0x017E. The built-in number formats are listed in 
        /// [ECMA-376] Part 4: Markup Language Reference, section 3.8.30.
        /// </summary>
        public ushort ifmt;

        public IFmtRecord(IStreamReader reader, RecordType id, ushort length)
            : base(reader, id, length)
        {
            // assert that the correct record type is instantiated
            Debug.Assert(this.Id == ID);

            // initialize class members from stream
            this.ifmt = reader.ReadUInt16();

            // assert that the correct number of bytes has been read from the stream
            Debug.Assert(this.Offset + this.Length == this.Reader.BaseStream.Position);
        }
    }
}
