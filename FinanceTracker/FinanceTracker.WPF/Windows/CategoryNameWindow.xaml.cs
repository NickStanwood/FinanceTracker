using System;
using System.ComponentModel;
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
using System.Runtime.CompilerServices;

namespace FinanceTracker.WPF
{
    /// <summary>
    /// Interaction logic for CategoryNameWindow.xaml
    /// </summary>
    public partial class CategoryNameWindow : Window, INotifyPropertyChanged
    {
        private string _categoryName = "";
        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; Notify(); }
        }
        public CategoryNameWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Notify([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
