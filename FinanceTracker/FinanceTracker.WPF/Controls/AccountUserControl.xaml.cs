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

namespace FinanceTracker.WPF
{
    /// <summary>
    /// Interaction logic for AccountUserControl.xaml
    /// </summary>
    public partial class AccountUserControl : UserControl
    {
        public static readonly DependencyProperty AccountProperty = DependencyProperty.Register("Account", typeof(AccountViewModel), typeof(AccountUserControl), new PropertyMetadata(null, AccountPropertyChanged));

        public AccountViewModel Account
        {
            get { return (AccountViewModel)GetValue(AccountProperty); }
            set { SetValue(AccountProperty, value); }
        }

        private void AccountPropertyChanged(AccountViewModel account)
        {

        }

        private static void AccountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AccountUserControl)d).AccountPropertyChanged((AccountViewModel)e.NewValue);
        }

        public AccountUserControl()
        {
            InitializeComponent();
        }
    }
}
