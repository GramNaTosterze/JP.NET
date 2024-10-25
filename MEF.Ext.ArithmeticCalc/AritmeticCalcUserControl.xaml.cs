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

namespace MEF.Ext.Arithmetic
{
    /// <summary>
    /// Interaction logic for AritmeticCalcUserControl.xaml
    /// </summary>
    public partial class AritmeticCalcUserControl : UserControl, ICalculator
    {
        public AritmeticCalcUserControl()
        {
            InitializeComponent();
            OutputTextBlock.Text = "";
        }

        public string Name { get; set; } = "Arithmetic Calculator";
        public UserControl GetUserControl() => this;

        private void SumButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var a = double.Parse(ATextBox.Text);
                var b = double.Parse(BTextBox.Text);
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
                var a = double.Parse(ATextBox.Text);
                var b = double.Parse(BTextBox.Text);
                OutputTextBlock.Text = $"{a * b}";
            }
            catch
            {
                MessageBox.Show("Invalid input");
            }
        }
    }
}
