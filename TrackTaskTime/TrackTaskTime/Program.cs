using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace TrackTaskTime
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var thisProcessName = Process.GetCurrentProcess().ProcessName;

                if (Process.GetProcesses().Count(p => p.ProcessName == thisProcessName) > 1)
                    return;

                var fileName = ConfigurationManager.AppSettings.Get("FileName");
                var sheetName = ConfigurationManager.AppSettings.Get("SheetName");

                var taskName = string.Empty;
                var someDate = DateTime.Now;
                var time = someDate.ToString("hh:mm tt");
                var date = someDate.ToString("dd/MMM/yyyy");

                if (args.Length > 0 && args[0].ToLower() == "tasknamerequired")
                {
                    Console.WriteLine("Time Initiated : " + DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    Console.WriteLine("Initiated by   : " + args[1] ?? "None");
                    Console.WriteLine("Please type the task you are going to do now: ");
                    taskName = Console.ReadLine();
                }
                else if(args.Length > 0)
                    taskName = args[0];


                var workbook = new XLWorkbook(fileName);
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
                    var newRowNumber = lr.RowNumber() + 1;
                    lr.CopyTo(ws.Cell(newRowNumber, 1));
                    var newTaskDetails = new List<string[]> {new[] {date, taskName, time, string.Empty}};
                    ws.Cell(newRowNumber, 1).InsertData(newTaskDetails);
                }

                workbook.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Console.WriteLine(ex.StackTrace);
                Console.ReadKey();
            }
            
        }
    }
}
