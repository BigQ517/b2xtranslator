/*
* Copyright (c) 2008, DIaLOGIKa
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*     * Redistributions of source code must retain the above copyright
*       notice, this list of conditions and the following disclaimer.
*     * Redistributions in binary form must reproduce the above copyright
*       notice, this list of conditions and the following disclaimer in the
*       documentation and/or other materials provided with the distribution.
*     * Neither the name of DIaLOGIKa nor the
*       names of its contributors may be used to endorse or promote products
*       derived from this software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY DIaLOGIKa ''AS IS'' AND ANY
* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
* DISCLAIMED. IN NO EVENT SHALL DIaLOGIKa BE LIABLE FOR ANY
* DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
* (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
* LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
* (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using DIaLOGIKa.b2xtranslator.CommonTranslatorLib;
using DIaLOGIKa.b2xtranslator.OpenXmlLib;
using DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat;
using DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.DataContainer;
using DIaLOGIKa.b2xtranslator.OpenXmlLib.SpreadsheetML;
using DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.Records;

namespace DIaLOGIKa.b2xtranslator.SpreadsheetMLMapping
{
    public class WorkbookMapping : AbstractOpenXmlMapping,
          IMapping<WorkBookData>
    {
        ExcelContext _xlsContext;
        WorkbookPart _workbookPart;

        /// <summary>
        /// Ctor 
        /// </summary>
        /// <param name="xlsContext">The excel context object</param>
        public WorkbookMapping(ExcelContext xlsContext, WorkbookPart workbookPart)
            : base(workbookPart.XmlWriter)
        {
            this._xlsContext = xlsContext;
            this._workbookPart = workbookPart;
        }

        /// <summary>
        /// The overload apply method 
        /// Creates the Workbook xml document 
        /// </summary>
        /// <param name="bsd">WorkSheetData</param>
        public void Apply(WorkBookData workbookData)
        {
            this._writer.WriteStartDocument();
            this._writer.WriteStartElement("workbook", OpenXmlNamespaces.SpreadsheetML);
            this._writer.WriteAttributeString("xmlns", Sml.Ns);
            this._writer.WriteAttributeString("xmlns", "r", "", OpenXmlNamespaces.Relationships);

            // fileVersion
            this._writer.WriteStartElement(Sml.Workbook.ElFileVersion, Sml.Ns);
            this._writer.WriteAttributeString(Sml.Workbook.AttrAppName, "xl");
            this._writer.WriteEndElement();

            // workbookPr
            this._writer.WriteStartElement(Sml.Workbook.ElWorkbookPr, Sml.Ns);
            if (workbookData.CodeName != null)
            {
                this._writer.WriteAttributeString(Sml.Workbook.AttrCodeName, workbookData.CodeName.codeName.Value);
            }
            this._writer.WriteEndElement();



            this._writer.WriteStartElement("sheets");

            // create the sheets
            foreach (var sheet in workbookData.boundSheetDataList)
            {
                OpenXmlPart sheetPart;
                switch (sheet.boundsheetRecord.dt)
                {
                    case BoundSheet8.SheetType.Worksheet:
                        {
                            var worksheetPart = this._workbookPart.AddWorksheetPart();
                            sheet.Convert(new WorksheetMapping(this._xlsContext, worksheetPart));
                            sheetPart = worksheetPart;
                        }
                        break;

                    case BoundSheet8.SheetType.Chartsheet:
                        {
                            var chartsheetPart = this._workbookPart.AddChartsheetPart();
                            sheet.Convert(new ChartsheetMapping(this._xlsContext, chartsheetPart));
                            sheetPart = chartsheetPart;
                        }
                        break;

                    default:
                        {
                            sheet.emtpyWorksheet = true;
                            var worksheetPart = this._workbookPart.AddWorksheetPart();
                            sheet.Convert(new WorksheetMapping(this._xlsContext, worksheetPart));
                            sheetPart = worksheetPart;
                        }
                        break;
                }

                this._writer.WriteStartElement("sheet");
                this._writer.WriteAttributeString("name", sheet.boundsheetRecord.stName.Value);
                this._writer.WriteAttributeString("sheetId", sheetPart.RelId.ToString());
                this._writer.WriteAttributeString("r", "id", OpenXmlNamespaces.Relationships, sheetPart.RelIdToString);
                this._writer.WriteEndElement();
            }
            this._writer.WriteEndElement();      // close sheetData 

            bool ParentTagWritten = false;
            if (workbookData.supBookDataList.Count != 0)
            {

                /*
                    <externalReferences>
                        <externalReference r:id="rId4" /> 
                        <externalReference r:id="rId5" /> 
                    </externalReferences>
                 */
                foreach (var var in workbookData.supBookDataList)
                {
                    if (!var.SelfRef)
                    {
                        if (!ParentTagWritten)
                        {
                            this._writer.WriteStartElement("externalReferences");
                            ParentTagWritten = true;
                        }
                        this._writer.WriteStartElement("externalReference");
                        this._writer.WriteAttributeString("r", "id", OpenXmlNamespaces.Relationships, var.ExternalLinkRef);
                        this._writer.WriteEndElement();
                    }
                }
                if (ParentTagWritten)
                {
                    this._writer.WriteEndElement();
                }
            }

            // write definedNames 

            if (workbookData.definedNameList.Count > 0)
            {
                //<definedNames>
                //<definedName name="abc" comment="test" localSheetId="1">Sheet1!$B$3</definedName>
                //</definedNames>
                this._writer.WriteStartElement("definedNames");

                foreach (var item in workbookData.definedNameList)
                {
                    if (item.rgce.Count > 0)
                    {
                        this._writer.WriteStartElement("definedName");
                        if (item.Name.Value.Length > 1)
                        {
                            this._writer.WriteAttributeString("name", item.Name.Value);
                        }
                        else
                        {
                            string internName = "_xlnm." + ExcelHelperClass.getNameStringfromBuiltInFunctionID(item.Name.Value);
                            this._writer.WriteAttributeString("name", internName);
                        }
                        if (item.itab > 0)
                        {
                            this._writer.WriteAttributeString("localSheetId", (item.itab - 1).ToString());
                        }
                        this._writer.WriteValue(FormulaInfixMapping.mapFormula(item.rgce, this._xlsContext));

                        this._writer.WriteEndElement();
                    }
                }
                this._writer.WriteEndElement();
            }

            // close tags 
            this._writer.WriteEndElement();      // close worksheet
            this._writer.WriteEndDocument();

            // flush writer 
            this._writer.Flush();
        }

    }
}
