using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using FinanceTracker.Models;

namespace FinanceTracker.WPF
{
    public enum MainWindowViewState
    {
        None,
        Goals,
        Budget,
        NetWorth,
        Account,
    }

    internal class FinancesViewModel : INotifyPropertyChanged
    {
        public LamdaCommand AddAccountCmd { get; set; }

        private object _accountLock = new object();
        public AccountNavigationViewModel Accounts { get; set; } = new AccountNavigationViewModel { Name= "Accounts"};
        public ObservableCollection<NavigationViewModel> NavigationItems { get; set; } = new ObservableCollection<NavigationViewModel>();

        private MainWindowViewState _viewState = MainWindowViewState.None;
        public MainWindowViewState ViewState { get { return _viewState; } set { _viewState = value; Notify(); } }
        public AccountViewModel _accountViewModel = new AccountViewModel();
        public AccountViewModel AccountViewModel { get { return _accountViewModel; } set { _accountViewModel = value; Notify(); } }

        public FinancesViewModel()
        {
            AddAccountCmd = new LamdaCommand(
                (obj) => true, 
                (obj) => AddAccount()
            );

            BindingOperations.EnableCollectionSynchronization(Accounts.AccountList, _accountLock);

            NavigationItems.Add(new NavigationViewModel { Name = "Budget"});
            NavigationItems.Add(new NavigationViewModel { Name = "Goals" });
            NavigationItems.Add(new NavigationViewModel { Name = "Net Worth" });
            NavigationItems.Add(Accounts);

            Task.Factory.StartNew(() => InitializeAccounts());

        }
        public async void InitializeAccounts()
        {
            List<AccountModel> accounts = await SQLiteContext.GetAllAccountsAsync();
            foreach(AccountModel account in accounts)
            {
                Accounts.AccountList.Add(account);
            }
        }

        public async void AddAccount()
        {
            AddAccountWindow wnd = new AddAccountWindow();
            if (wnd.ShowDialog() == true)
            {
                AccountModel? am = await SQLiteContext.AddAccount(wnd.AccountName);
                if(am != null)
                {
                    Accounts.AccountList.Add(am);

                    Random rand = new Random();
                    am.Balance = rand.NextDouble()*10000;
                    for (int i = 0; i < 100; i++)
                    {
                        double transVal = rand.NextDouble() * -100.0;
                        await SQLiteContext.AddTransaction(am.Id, DateTime.Now - TimeSpan.FromHours(i*8), transVal, $"Transaction {i}", null);
                        am.Balance += transVal;
                    }
                    await SQLiteContext.UpdateAccount(am);
                }
            }
        }

        public void NavigationStateChanged(object sender, NavigationViewModel m)
        {
            ViewState = MainWindowViewState.None;
        }
        public void NavigationStateChanged(object sender, AccountModel m)
        {
            AccountViewModel = new AccountViewModel(m);
            ViewState = MainWindowViewState.Account;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Notify([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
