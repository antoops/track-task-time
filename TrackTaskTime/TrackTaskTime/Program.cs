using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace TrackTaskTime
{
    class Program
    {

        static void Main(string[] args)
        {
            var fileName = ConfigurationManager.AppSettings.Get("FileName");
            var sheetName = ConfigurationManager.AppSettings.Get("SheetName");

            var taskName = string.Empty;
            var someDate = DateTime.Now;
            var time = someDate.ToString("hh:mm tt");
            var date = someDate.ToString("dd/MMM/yyyy");

            int newRowNumber;
            if (args.Length > 0)
                taskName = args[0];

            XLWorkbook workbook = new XLWorkbook(fileName);
            var ws = workbook.Worksheets.Worksheet(sheetName);
            var range = ws.RangeUsed();
            var lr = range.LastRow();

            //Updating the completed date
            var taskToBeCompleted = new List<string>
            {
                time
            };
            ws.Cell(lr.RowNumber(), 4).InsertData(taskToBeCompleted);


            //copying last row to new row if its a new task
            if (!string.IsNullOrWhiteSpace(taskName))
            {
                newRowNumber = lr.RowNumber() + 1;
                lr.CopyTo(ws.Cell(newRowNumber, 1));
                var newTaskDetails = new List<string[]>();
                newTaskDetails.Add(new string[] { date, taskName, time, string.Empty });
                ws.Cell(newRowNumber, 1).InsertData(newTaskDetails);
            }

            workbook.Save();
        }
    }
}
