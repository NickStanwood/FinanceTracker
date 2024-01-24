using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Models;

namespace FinanceTracker.WPF
{
    internal class AccountNavigationViewModel : NavigationViewModel
    {
        public ObservableCollection<AccountModel> AccountList { get; set; } = new ObservableCollection<AccountModel>();
    }

    internal class NavigationViewModel
    {
        public string Name { get; set; }
    }
}
