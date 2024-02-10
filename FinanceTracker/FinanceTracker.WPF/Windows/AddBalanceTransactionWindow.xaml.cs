using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for AddBalanceTransactionWindow.xaml
    /// </summary>
    public partial class AddBalanceTransactionWindow : Window, INotifyPropertyChanged
    {
        private DateTime _date;
        public DateTime Date { get { return _date; } set { _date = value; Notify(); } }

        private double _value;
        public double Value { get { return _value; } set { _value = value; Notify(); } }
        public AddBalanceTransactionWindow()
        {
            InitializeComponent();
            DataContext = this;
            Date = DateTime.Now;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Notify([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
