using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MEF.Ext;

[InheritedExport(typeof(ICalculator))]
public interface ICalculator
{
    string Name { get; set; }
    UserControl GetUserControl();
}