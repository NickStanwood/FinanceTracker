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

        private double _transactionFrequency =0.0;
        public double TransactionFrequency { get { return _transactionFrequency; } private set { _transactionFrequency = value; Notify(); } }

        private double _averagePurchaseCost = 0.0;
        public double AveragePurchaseCost { get { return _averagePurchaseCost; } private set { _averagePurchaseCost = value; Notify(); } }

        public ObservableCollection<CategoryStats> Categories { get; set; } = new ObservableCollection<CategoryStats>();
        public ObservableCollection<TransactionModel> Transactions { get; set; } = new ObservableCollection<TransactionModel>();
        public LamdaCommand AddTransactions { get; set; }
        public LamdaCommand UpdateBalance { get; set; }
        public AccountViewModel() : base() { }
        public AccountViewModel(AccountModel model) : base(model)
        {
        }

        protected override async void Initialize()
        {
            if (_m.Id == Guid.Empty)
                return;

            AddTransactions = new LamdaCommand(
                (obj) => true,
                (obj) => ShowTransactionsDialog()
            );

            UpdateBalance = new LamdaCommand(
                (obj) => true,
                (obj) => ShowTransactionsDialog()
            );

            Notify(nameof(Balance));
            Notify(nameof(AccountName));
            Notify(nameof(CurrencyType));

            List<TransactionModel> transactions = await SQLiteContext.GetAllAccountTransactionsAsync(_m.Id);
            foreach(TransactionModel transaction in transactions)
            {
                Transactions.Add(transaction);
            }

            TimeSpan avgPeriod = TimeSpan.FromDays(30);
            double spent = 0.0;
            List<TransactionModel> lastMonthTrans = await SQLiteContext.GetAllAccountTransactionsAsync(_m.Id, DateTime.Now - avgPeriod);
            if(lastMonthTrans != null && lastMonthTrans.Count > 0)
            {
                foreach (TransactionModel transaction in lastMonthTrans)
                {
                    spent += transaction.DollarValue;
                }
                AveragePurchaseCost = spent / lastMonthTrans.Count;
                TransactionFrequency = lastMonthTrans.Count / avgPeriod.TotalDays;
            }
            else
            {
                AveragePurchaseCost = 0.0;
                TransactionFrequency = 0.0;
            }
            

            List<CategoryModel> categories = await SQLiteContext.GetAllCategories();
            foreach(CategoryModel category in categories)
            {
                Categories.Add(new CategoryStats(category, transactions));
            }
        }

        private void ShowTransactionsDialog()
        {
            AddRawTransactionWindow transWin = new AddRawTransactionWindow(_m);
            transWin.ShowDialog();
        }
        private void ShowBalanceDialog()
        {
            //TODO
        }

    }
}
