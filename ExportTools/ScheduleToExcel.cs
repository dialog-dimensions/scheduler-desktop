using System.Globalization;
using System.Windows;
using ClosedXML.Excel;
using Microsoft.Win32;
using SchedulerDesktop.Models.Entities;
using SchedulerDesktop.Models.ScheduleEngine;

namespace SchedulerDesktop.ExportTools;

public static class ScheduleToExcel
{
    private static void ExportScheduleToExcel(ScheduleData data, string filePath)
    {
        var schedule = data.Schedule;
        var employees = data.Employees.ToList();
        
        using var workbook = new XLWorkbook();
        var ws1 = workbook.AddWorksheet($"By Day");
        var ws2 = workbook.AddWorksheet("By Employee");

        ws1.RightToLeft = true;
        ws2.RightToLeft = true;

        // Assuming Sunday as the first day of the week
        var culture = new CultureInfo("he-IL");
        var dayNames = culture.DateTimeFormat.ShortestDayNames;

        PopulateGeneralWeeklySchedule(ws1, schedule, dayNames, employees);
        PopulateEmployeeSchedule(ws2, schedule, employees);

        workbook.SaveAs(filePath);
    }
    
    private static void PopulateGeneralWeeklySchedule(IXLWorksheet worksheet, Schedule schedule, string[] dayNames, IList<Employee> employees)
    {
        var scheduleStartDisplayDate = schedule.StartDateTime.Date.AddDays(schedule.StartDateTime.Hour > 5 ? -1 : 0);
        
        var shiftsByDay = schedule
            .GroupBy(s => s.DisplayDate.Date)
            .OrderBy(g => g.Key)
            .ToList();

        // Header
        worksheet.Cell(1, 1).Value = $"Schedule: {schedule.StartDateTime.ToShortDateString()} - {schedule.EndDateTime.ToShortDateString()}";
        worksheet.Range(1, 1, 1, shiftsByDay.Count).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        
        foreach (var group in shiftsByDay)
        {
            var currentRow = 3;
            var currentDisplayDate = group.Key;
            var dayOfWeek = (int)currentDisplayDate.DayOfWeek;
            var currentRelativeDay = (int)currentDisplayDate.Subtract(scheduleStartDisplayDate).TotalDays;
            worksheet.Cell(2, currentRelativeDay + 1).Value = group.Key.ToString("dd/MM") + " " + dayNames[dayOfWeek];
        
            foreach (var shift in group)
            {
                // Populate shift details. Adjust cell size and merge cells as needed.
                var cell = worksheet.Cell(currentRow++, currentRelativeDay + 1);
                cell.Value = $"{shift.StartDateTime:dd/MM HH:mm} - {shift.EndDateTime:HH:mm}\n{employees.First(emp => emp.Id == shift.EmployeeId).Name}";
                // Example of adjusting cell size; customize as needed
                cell.Style.Alignment.WrapText = true;
            }
        }

        // Adjust column widths and row heights as needed
        worksheet.Columns().AdjustToContents();
        worksheet.Rows().AdjustToContents();
    }
    
    private static void PopulateEmployeeSchedule(IXLWorksheet worksheet, Schedule schedule, IList<Employee> employees)
    {
        var shiftsByEmployee = schedule.GroupBy(s => employees.First(emp => emp.Id == s.EmployeeId).Name).OrderBy(g => g.Key);

        int currentColumn = 1;
        foreach (var group in shiftsByEmployee)
        {
            worksheet.Cell(1, currentColumn).Value = group.Key;
            int currentRow = 2;
            foreach (var shift in group.OrderBy(s => s.StartDateTime))
            {
                // Populate shift details. Adjust cell size and merge cells as needed.
                var cell = worksheet.Cell(currentRow++, currentColumn);
                cell.Value = $"{shift.StartDateTime:dd/MM HH:mm} - {shift.EndDateTime:HH:mm}";
                // Example of adjusting cell size; customize as needed
                cell.Style.Alignment.WrapText = true;
            }
            currentColumn++;
        }

        // Adjust column widths and row heights as needed
        worksheet.Columns().AdjustToContents();
        worksheet.Rows().AdjustToContents();
    }
    
    
    public static void ExportScheduleWithDialog(ScheduleData data)
    {
        var saveFileDialog = new SaveFileDialog
        {
            DefaultExt = ".xlsx",
            Filter = "Excel Workbook (.xlsx)|*.xlsx",
            FileName = "Schedule_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx", // Default file name
            Title = "Export Schedule to Excel"
        };

        bool? result = saveFileDialog.ShowDialog();

        if (result == true)
        {
            string filePath = saveFileDialog.FileName;
            try
            {
                ExportScheduleToExcel(data, filePath);
                MessageBox.Show("Schedule exported successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to export schedule.\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}