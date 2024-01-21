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
    internal class FinancesViewModel : INotifyPropertyChanged
    {
        public List<AccountModel> Accounts = new List<AccountModel>();
        
        public FinancesViewModel()
        {
            Accounts.Add(new AccountModel { Name = "Account 1" });
            Accounts.Add(new AccountModel { Name = "Account 2"});
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Notify([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
