﻿

using DIaLOGIKa.b2xtranslator.StructuredStorage.Reader;
using DIaLOGIKa.b2xtranslator.Tools;

namespace DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.Structures
{
    /// <summary>
    /// This structure specifies a Font record in the file.
    /// </summary>
    public class FontInfo
    {
        /// <summary>
        /// A bit that specifies whether the fonts are scaled.
        /// </summary>
        public bool fScaled;

        public ushort ifnt;
        // TODO: implement FontIndex???

        public FontInfo(IStreamReader reader)
        {
            this.fScaled = Utils.BitmaskToBool(reader.ReadUInt16(), 0x0001);
            this.ifnt = reader.ReadUInt16();
        }
    }
}
