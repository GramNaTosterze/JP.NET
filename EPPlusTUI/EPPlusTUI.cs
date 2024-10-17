namespace EPPlusTUI;

using System;
using Terminal.Gui;

public partial class EPPlusTUI
{
    public EPPlusTUI()
    {
        InitializeComponent();

        errorLabel.Text = string.Empty;
        
        generateButton.Clicked += () =>
        {
            try
            {
                var filePath = excelFileField.Text.ToString() ?? "scanDir.xlsx";
                var dir = scanDirField.Text.ToString() ?? ".";
                var d = uint.Parse(dField.Text.ToString() ?? "1");

                var excel = new ExcelHandler(filePath, dir, d);
                excel.ListDirectory();
                excel.Statistics();
                excel.Save();
                errorLabel.Text += $"File {filePath} has been saved successfully.";
            }
            catch (Exception e)
            {
                errorLabel.Text += $"{e.Message}\n";
            }
        };
    }
}