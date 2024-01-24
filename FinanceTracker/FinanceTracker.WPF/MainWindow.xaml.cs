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
using FinanceTracker.Models;

namespace FinanceTracker.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FinancesViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = DataContext as FinancesViewModel;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            NavigationViewModel? navModel = e.NewValue as NavigationViewModel;
            if(navModel != null)
            {
                _viewModel.NavigationStateChanged(sender, navModel);
                return;
            }

            AccountModel? accModel = e.NewValue as AccountModel;
            if(accModel != null)
            {
                _viewModel.NavigationStateChanged(sender, accModel);
                return;
            }
        }
    }
}
