using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;

namespace TrackTaskTime
{
    class Program
    {

        static void Main(string[] args)
        {
            var fileName = ConfigurationManager.AppSettings.Get("FileName");
            var sheetName = ConfigurationManager.AppSettings.Get("SheetName");

            XLWorkbook workbook = new XLWorkbook(fileName);
            var ws = workbook.Worksheets.Worksheet(sheetName);
            var range = ws.RangeUsed();
            var lr = range.LastRow();
            //copying last row to new row
            var newRowNumber = lr.RowNumber() + 1;
            lr.CopyTo(ws.Cell(newRowNumber, 1));

            //Updating the completed date
            var someDate = DateTime.Now;
            var time = someDate.ToString("hh:mm tt");
            var date = someDate.ToString("dd/MMM/yyyy");
            var listOfStrings = new List<String>();
            listOfStrings.Add(time);
            ws.Cell(lr.RowNumber(), 4).InsertData(listOfStrings);


            //Updating newly added row
            var taskName = "New Task";
            if (args.Length > 0)
                taskName = args[0];
            var listOfArr = new List<string[]>();
            listOfArr.Add(new string[] { date, taskName, time });
            ws.Cell(newRowNumber, 1).InsertData(listOfArr);
            
            workbook.Save();
        }
    }
}
