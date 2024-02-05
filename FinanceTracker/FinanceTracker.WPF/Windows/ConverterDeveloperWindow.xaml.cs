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
using System.Windows.Shapes;

namespace FinanceTracker.WPF
{
    /// <summary>
    /// Interaction logic for ConverterDeveloperWindow.xaml
    /// </summary>
    public partial class ConverterDeveloperWindow : Window
    {
        public ConverterDeveloperWindow(ConverterDeveloperViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
