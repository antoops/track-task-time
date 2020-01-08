using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Windows.Forms;

namespace LogMe
{
    public partial class frmLogMe : Form
    {
        public frmLogMe()
        {
            InitializeComponent();
        }

        private void btTask_Click(object sender, EventArgs e)
        {
            var fileName = DateTime.Now.ToString().Replace('/','-').Replace(':','-')+".xlsx";

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Test Sheet" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();
            }
        }
    }
}