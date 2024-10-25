using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MEF.Ext.Set
{
    /// <summary>
    /// Interaction logic for SetCalcUserControl.xaml
    /// </summary>
    public partial class SetCalcUserControl : UserControl, ICalculator
    {
        public SetCalcUserControl()
        {
            InitializeComponent();
            OutputTextBlock.Text = "";
        }

        public string Name { get; set; } = "Set Calculator";
        public UserControl GetUserControl() => this;

        private void SumButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var a = ATextBox.Text;
                var b = BTextBox.Text;
                OutputTextBlock.Text = $"{a + b}";
            }
            catch
            {
                MessageBox.Show("Invalid input");
            }
        }

        private void ProductButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var a = (ATextBox.Text);
                var b = (BTextBox.Text);
                var product = (a + b).Where(x => a.Contains(x) && b.Contains(x));
                OutputTextBlock.Text = $"{string.Join("",product)}";
            }
            catch
            {
                MessageBox.Show("Invalid input");
            }
        }
    }
}
