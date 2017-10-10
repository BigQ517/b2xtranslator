﻿using System.Diagnostics;
using DIaLOGIKa.b2xtranslator.StructuredStorage.Reader;

namespace DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.Records
{
    /// <summary>
    /// This record specifies which part of the axis is specified by the LineFormat record that follows.
    /// </summary>
    [BiffRecord(RecordType.AxisLine)]
    public class AxisLine : BiffRecord
    {
        public const RecordType ID = RecordType.AxisLine;

        public enum AxisPart : ushort
        {
            /// <summary>
            /// The axis line itself
            /// </summary>
            AxisLine = 0x0,

            /// <summary>
            /// The major gridlines along the axis
            /// </summary>
            MajorGridlines = 0x1,

            /// <summary>
            /// The minor gridlines along the axis
            /// </summary>
            MinorGridlines = 0x2,

            /// <summary>
            /// The walls or floor of a 3-D chart
            /// </summary>
            WallOrFloor3D = 0x3
        }

        /// <summary>
        /// An unsigned integer that specifies which part of the axis is defined 
        /// by the LineFormat record that follows. 
        /// 
        /// MUST be unique among all other id field values in AxisLine records in the current axis. 
        /// MUST be greater than the id field values in preceding AxisLine records in the current axis.
        /// </summary>
        public AxisPart axisId;

        public AxisLine(IStreamReader reader, RecordType id, ushort length)
            : base(reader, id, length)
        {
            // assert that the correct record type is instantiated
            Debug.Assert(this.Id == ID);

            // initialize class members from stream
            this.axisId = (AxisPart)reader.ReadUInt16();

            // assert that the correct number of bytes has been read from the stream
            Debug.Assert(this.Offset + this.Length == this.Reader.BaseStream.Position);
        }
    }
}
