using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Models;

namespace FinanceTracker.WPF
{
    public class AccountViewModel : ViewModelBase<AccountModel>
    {
        public double Balance { get { return _m.Balance; } }
        public string AccountName { get { return _m.Name; } }
        public string CurrencyType { get { return _m.CurrencyType; } }

        public ObservableCollection<TransactionModel> Transactions { get; set; } = new ObservableCollection<TransactionModel>();
        public AccountViewModel() : base() { }
        public AccountViewModel(AccountModel model) : base(model)
        {
        }

        protected override async void Initialize()
        {
            if (_m.Id == Guid.Empty)
                return;

            List<TransactionModel> transactions = await SQLiteContext.GetAllAccountTransactionsAsync(_m.Id);
            foreach(TransactionModel transaction in transactions)
            {
                Transactions.Add(transaction);
            }
        }

    }
}
