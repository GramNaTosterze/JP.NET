using System.Windows.Controls;

namespace MEF.CalcInterface;

public interface ICalculator
{
    string Name { get; set; }
    UserControl GetUserControl();
}