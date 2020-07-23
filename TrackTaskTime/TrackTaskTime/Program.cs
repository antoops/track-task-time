using ClosedXML.Excel;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace TrackTaskTime
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly string QueueFileName = Application.StartupPath +"\\"+ @"QueueData.txt";
        private static void Main(string[] args)
        {
            try
            {
                var thisProcessName = Process.GetCurrentProcess().ProcessName;

                if (Process.GetProcesses().Count(p => p.ProcessName == thisProcessName) > 1)
                    return;

                var fileName = ConfigurationManager.AppSettings.Get("FileName");
                var sheetName = ConfigurationManager.AppSettings.Get("SheetName");
                Logger.Info($"{sheetName} sheet in {fileName} file will be updated ");

                var newTaskName = string.Empty;
                var someDate = DateTime.Now;
                var time = someDate.ToString("hh:mm tt");
                var date = someDate.ToString("dd/MMM/yyyy");

                newTaskName = GetTaskName(args, newTaskName, time, date);

                XLWorkbook workbook = null;
                try
                {
                    workbook = new XLWorkbook(fileName);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("because it is being used by another process"))
                    {
                        Logger.Error($"File is already in use, so will queue this command");
                        WriteToQueue(newTaskName, date, time);
                    }
                    return;
                }
                //file is able to edit
                //read from queue and append file
                var queueData = File.ReadAllLines(QueueFileName);
                var ws = workbook.Worksheets.Worksheet(sheetName);
                var range = ws.RangeUsed();
                var lr = range.LastRow();
                int rowNumberToEdit = lr.RowNumber();
                if (queueData.Length>0)
                {
                    Logger.Info($"Queue have {queueData.Length} records");
                    foreach (var queueItem in queueData)
                    {
                        Logger.Info($"Writing queue item = {queueItem} at row number {rowNumberToEdit}");

                        var queueObject = ConvertToQueueObject(queueItem);

                        var lastTime = ws.Cell(rowNumberToEdit, 4).Value.ToString();
                        if (string.IsNullOrWhiteSpace(lastTime))
                        {
                            Logger.Info($"Last task's end time will be updated.");
                            ws.Cell(rowNumberToEdit, 4).InsertData(
                            new List<string>
                            {
                                queueObject.StartTime
                            });
                        }
                        
                        rowNumberToEdit++;
                        if (!string.IsNullOrWhiteSpace(queueObject.TaskName))
                        {
                            lr.CopyTo(ws.Cell(rowNumberToEdit, 1));
                            var newTaskDetails = new List<string[]> { new[] { queueObject.Date, queueObject.TaskName,
                                queueObject.StartTime, string.Empty } };
                            ws.Cell(rowNumberToEdit, 1).InsertData(newTaskDetails);
                        }
                    }
                    ClearQueue();
                }
                
                //Updating the completed date
                var taskToBeCompleted = new List<string>
                    {
                        time
                    };
                var lastRowTime = ws.Cell(rowNumberToEdit, 4).Value.ToString();
                if (string.IsNullOrWhiteSpace(lastRowTime))
                {
                    Logger.Info($"Last task's end time will be updated.");
                    ws.Cell(rowNumberToEdit, 4).InsertData(taskToBeCompleted);
                }
                else
                    Logger.Info($"Last row time is <{lastRowTime}>, so will skip this windup command");

                rowNumberToEdit++;
                //copying last row to new row if its a new task
                if (!string.IsNullOrWhiteSpace(newTaskName))
                {
                    Logger.Info($"{newTaskName} is going to start on {date} at {time}");
                    lr.CopyTo(ws.Cell(rowNumberToEdit, 1));
                    var newTaskDetails = new List<string[]> { new[] { date, newTaskName, time, string.Empty } };
                    ws.Cell(rowNumberToEdit, 1).InsertData(newTaskDetails);
                }
                
                workbook.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            
        }

        private static void ClearQueue()
        {
            Logger.Info($"Queue is cleared.");
            File.WriteAllText(QueueFileName, String.Empty);
        }

        private static QueueObject ConvertToQueueObject(string queueItem)
        {
            QueueObject queueObject = null;
            var dataItems = queueItem.Split(',');
            if(dataItems.Length==3)
            {
                queueObject = new QueueObject
                {
                    TaskName = dataItems[0],
                    Date = dataItems[1],
                    StartTime = dataItems[2]
                };
            }
            return queueObject;
        }

        private static string GetTaskName(string[] args, string taskName, string time, string date)
        {
            if (args.Length > 0 && args[0].ToLower() == "tasknamerequired")
            {
                Console.WriteLine("Time Initiated : " + DateTime.Now.ToString(CultureInfo.InvariantCulture));
                Console.WriteLine("Initiated by   : " + args[1] ?? "None");
                Console.WriteLine("Please type the task you are going to do now: ");
                taskName = Console.ReadLine();
                Logger.Info($"{taskName} is going to start on {date} at {time}");
            }
            else if (args.Length > 0)
                taskName = args[0];
            return taskName;
        }

        private static void WriteToQueue(string taskName, string date, string time)
        {
            Logger.Info($"{taskName} is going to queued on {date} at {time}");
            var data = taskName + "," + date + "," + time;
            File.AppendAllText(QueueFileName, data + Environment.NewLine);
        }
    }

    public class QueueObject
    {
        public string TaskName;
        public string Date;
        public string StartTime;
    }
}
