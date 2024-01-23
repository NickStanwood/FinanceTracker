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
        public ObservableCollection<TransactionModel> Transactions = new ObservableCollection<TransactionModel>();
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
