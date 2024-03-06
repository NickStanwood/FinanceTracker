using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FinanceTracker.Models;
using FinanceTracker.Core;

namespace FinanceTracker.WPF
{
    //summary tab of accountViewModel
    public partial class AccountViewModel : ViewModelBase<AccountModel>
    {
        public string AccountName { get { return _m.Name; } }
        public string CurrencyType { get { return _m.CurrencyType; } }

        private double _balance;
        public double Balance { get { return _balance; } set { _balance = value; Notify(); } }

        private double _transactionFrequency =0.0;
        public double TransactionFrequency { get { return _transactionFrequency; } private set { _transactionFrequency = value; Notify(); } }

        private double _averagePurchaseCost = 0.0;
        public double AveragePurchaseCost { get { return _averagePurchaseCost; } private set { _averagePurchaseCost = value; Notify(); } }

        public ObservableCollection<DateTimeDoublePoint> BalanceOverTime { get; set; } = new ObservableCollection<DateTimeDoublePoint>();
        public DateTimeDoublePoint MinY { get; set; } = new DateTimeDoublePoint(DateTime.MinValue, -5000.0);
        public DateTimeDoublePoint MaxY { get; set; } = new DateTimeDoublePoint(DateTime.MinValue, 1000.0);

        public ObservableCollection<CategoryStats> Categories { get; set; } = new ObservableCollection<CategoryStats>();
        public ObservableCollection<TransactionModel> Transactions { get; set; } = new ObservableCollection<TransactionModel>();
        public LamdaCommand UpdateBalance { get; set; }
        public AccountViewModel() : base() { }
        public AccountViewModel(AccountModel model) : base(model)
        {
            Initialize_Add();
        }

        protected override async void Initialize()
        {
            if (_m.Id == Guid.Empty)
                return;

            UpdateBalance = new LamdaCommand(
                (obj) => true,
                (obj) => ShowBalanceDialog()
            );

            Notify(nameof(AccountName));
            Notify(nameof(CurrencyType));

            //create list of all transactions
            List<TransactionModel> transactions = await SQLiteContext.GetAllAccountTransactionsAsync(_m.Id);
            foreach(TransactionModel transaction in transactions)
            {
                Transactions.Add(transaction);
                if (transaction.Balance != null)
                    BalanceOverTime.Add(new DateTimeDoublePoint(transaction.Date, (double)transaction.Balance));
            }
            Notify(nameof(BalanceOverTime));

            //average out the transactions in the past month
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

            //get account balance
            TransactionModel? b = await SQLiteContext.GetAccountBalanceAsync(_m.Id);
            if (b != null)
                Balance = b.Balance != null ? (double)b.Balance : 0.0;

            //TODO
            List<CategoryModel> categories = await SQLiteContext.GetAllCategories();
            foreach(CategoryModel category in categories)
            {
                Categories.Add(new CategoryStats(category, transactions));
            }
        }

        private async void ShowBalanceDialog()
        {
            AddBalanceTransactionWindow addBalanceDialog = new AddBalanceTransactionWindow();
            if(addBalanceDialog.ShowDialog() == true)
            {
                TransactionModel trans = new TransactionModel
                {
                    AccountId = _m.Id,
                    Balance = addBalanceDialog.Value,
                    Date = addBalanceDialog.Date,
                    Name = "[Manual Account Balance Update]",
                    DollarValue = 0.0,
                };
                TransactionModel? tm = await SQLiteContext.AddTransaction(trans);
            }
        }
    }
}
