using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Models;
using FinanceTracker.Core;

namespace FinanceTracker.WPF
{
    public class AccountViewModel : ViewModelBase<AccountModel>
    {
        public double Balance { get { return _m.Balance; } }
        public string AccountName { get { return _m.Name; } }
        public string CurrencyType { get { return _m.CurrencyType; } }
        public TimeSpan TransactionFrequency { get; private set; }
        public double AveragePurchaseCost { get; private set; }

        public ObservableCollection<CategoryStats> Categories { get; set; } = new ObservableCollection<CategoryStats>();
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

            TimeSpan avgPeriod = TimeSpan.FromDays(30);
            double spent = 0.0;
            List<TransactionModel> lastMonthTrans = await SQLiteContext.GetAllAccountTransactionsAsync(_m.Id, DateTime.Now - avgPeriod);
            foreach (TransactionModel transaction in lastMonthTrans)
            {
                spent += transaction.DollarValue;
            }
            AveragePurchaseCost = spent / lastMonthTrans.Count;
            TransactionFrequency = avgPeriod / lastMonthTrans.Count;

            List<CategoryModel> categories = await SQLiteContext.GetAllCategories();
            foreach(CategoryModel category in categories)
            {
                Categories.Add(new CategoryStats(category, transactions));
            }
        }

    }
}
