﻿

using DIaLOGIKa.b2xtranslator.StructuredStorage.Reader;

namespace DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.Structures
{
    public class XmlTkTpb
    {
        public XmlTkBlob textPropsStream;

        public XmlTkTpb(IStreamReader reader)
        {
            this.textPropsStream = new XmlTkBlob(reader);   
        }
    }
}
