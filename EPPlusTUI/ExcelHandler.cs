using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;

namespace EPPlusTUI;

public class ExcelHandler
{
    private readonly ExcelPackage _excelPackage;
    private readonly string _dir;
    private uint _d;
    private List<FileInfo>? _files = null;
    
    
    public ExcelHandler(string excelFilePath, string dir, uint d) 
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        var spreadsheet = new FileInfo(excelFilePath); 
        _dir = dir;
        _d = d;
        
        try
        {
            File.Delete(spreadsheet.FullName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine("Failed to delete excel file.");
            throw;
        }
        
        _excelPackage = new ExcelPackage(spreadsheet);
        _excelPackage.Workbook.Properties.Title = "EPPlusTUI Example";
        _excelPackage.Workbook.Properties.Author = "Krzysiu";
        _excelPackage.Workbook.Properties.Company = "Politechnika Gdańska";
    }

    public void Save()
    {
        _excelPackage?.Save();
    }

    public void ListDirectory()
    {
        _excelPackage.Workbook.Worksheets.Add("Struktura katalogu");
        var ws = _excelPackage.Workbook.Worksheets["Struktura katalogu"];
        ws.Cells[2, 2].Value = "Nazwa";
        ws.Cells[2, 2].Style.Font.Bold = true;
        ws.Cells[2, 3].Value = "Rozszerzenie";
        ws.Cells[2, 3].Style.Font.Bold = true;
        ws.Cells[2, 4].Value = "Rozmiar";
        ws.Cells[2, 4].Style.Font.Bold = true;
        ws.Cells[2, 5].Value = "Atrybuty";
        ws.Cells[2, 5].Style.Font.Bold = true;
        
        int row = 3;
        _files = ListFiles(ws, _dir, 3, ref row);
        ws.Cells.AutoFitColumns();
    }

    public void Statistics()
    {
        if (_files == null) 
            ListDirectory();
        if (_files == null)
            return;
        
        _files.Sort((a, b) => -1*a.Length.CompareTo(b.Length));

        var row = 3;
        
        _excelPackage.Workbook.Worksheets.Add("Statystyki");
        var ws = _excelPackage.Workbook.Worksheets["Statystyki"];
        ws.Cells[2, 2].Value = "nazwa";
        ws.Cells[2, 2].Style.Font.Bold = true;
        ws.Cells[2, 3].Value = "rozmiar";
        ws.Cells[2, 3].Style.Font.Bold = true;
        foreach (var file in _files[..10]) 
        {
            ws.Cells[row, 2].Value = file.Name;
            ws.Cells[row, 3].Value = file.Length;
            row++;
        }

        var extensions = (from file in _files[..10] select file.Extension).Distinct();

        row += 4;

        ws.Cells[row-1, 2].Value = "Rozszerzenie";
        ws.Cells[row-1, 2].Style.Font.Bold = true;
        ws.Cells[row-1, 3].Value = "Ilość występowania";
        ws.Cells[row-1, 3].Style.Font.Bold = true;
        var startingRow = row;
        foreach (var extension in extensions)
        {
            ws.Cells[row, 2].Value = extension;
            var eFiles = _files[..10].Where(f => f.Extension == extension);
            ws.Cells[row, 3].Value = eFiles.Count();
            ws.Cells[row, 4].Value = eFiles.Sum(f => f.Length);
            row++;
        }

        ws.Cells.AutoFitColumns();
        
        DrawChart(ws, "% Rozszerzeń ilościowo", startingRow, startingRow+ extensions.Count()-1, 3);
        DrawChart(ws, "% Rozszerzeń wg rozmiaru", startingRow, startingRow+ extensions.Count()-1, 4);
    }
    
    private List<FileInfo> ListFiles(ExcelWorksheet ws, string path, uint d,ref int row, string subdir = "", uint outline = 1)
    {
        var files = new List<FileInfo>();
        
        foreach (var dir in Directory.GetDirectories(path))
        {
            var dirInfo = new FileInfo(dir);
            ws.Cells[row, 2].Value = $"{subdir}{dirInfo.Name}/";
            ws.Cells[row, 5].Value = dirInfo.Attributes;
            ws.Row(row).OutlineLevel = (int)outline;
            row++;
            if (d > 0)
            {
                files = ListFiles(ws, dirInfo.FullName, d - 1, ref row, $"{subdir}{dirInfo.Name}/", outline+1);
            }
        }
        
        foreach (var file in Directory.GetFiles(path))
        {
            var fileInfo = new FileInfo(file);
            ws.Cells[row, 2].Value = $"{subdir}{fileInfo.Name}";
            ws.Cells[row, 3].Value = fileInfo.Extension;
            ws.Cells[row, 4].Value = $"{fileInfo.Length/1024}MB";
            ws.Cells[row, 5].Value = fileInfo.Attributes;
            ws.Row(row).OutlineLevel = (int)outline;
            row++;
            
            files.Add(fileInfo);
        }
        
        return files;
    }
    
    private void DrawChart(ExcelWorksheet ws, string title, int startingRow, int endingRow, int dataCol)
    {
        var chart = (ws.Drawings.AddChart(title, eChartType.Pie3D) as ExcelPieChart);
        if (chart == null)
            return;
        chart.Title.Text = title;
        chart.SetPosition(0, 5, 5, 5);
        chart.SetSize(600, 300);
    
    
        var valAdd = new ExcelAddress(startingRow, dataCol, endingRow, dataCol);
        chart.Series.Add(valAdd.Address, $"B{startingRow}:B{endingRow}");
    
        chart.DataLabel.ShowCategory = true;
        chart.DataLabel.ShowPercent = true;
    
        chart.Legend.Border.LineStyle = eLineStyle.Solid;
        chart.Legend.Border.Fill.Style = eFillStyle.SolidFill;
        chart.Legend.Border.Fill.Color = Color.DarkBlue;
    }
}