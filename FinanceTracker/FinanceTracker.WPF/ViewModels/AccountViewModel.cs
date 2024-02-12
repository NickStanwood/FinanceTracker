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
    public class AccountViewModel : ViewModelBase<AccountModel>
    {
        public string AccountName { get { return _m.Name; } }
        public string CurrencyType { get { return _m.CurrencyType; } }

        private double _balance;
        public double Balance { get { return _balance; } set { _balance = value; Notify(); } }

        private double _transactionFrequency =0.0;
        public double TransactionFrequency { get { return _transactionFrequency; } private set { _transactionFrequency = value; Notify(); } }

        private double _averagePurchaseCost = 0.0;
        public double AveragePurchaseCost { get { return _averagePurchaseCost; } private set { _averagePurchaseCost = value; Notify(); } }

        private string _rawTransFilePath;
        public string RawTransFilePath { get { return _rawTransFilePath; } set { _rawTransFilePath = value; Notify(); } }

        public ObservableCollection<CategoryStats> Categories { get; set; } = new ObservableCollection<CategoryStats>();
        public ObservableCollection<TransactionModel> Transactions { get; set; } = new ObservableCollection<TransactionModel>();
        public ObservableCollection<ConvertTransactionViewModel> ConvertTransactions { get; set; } = new ObservableCollection<ConvertTransactionViewModel>();
        public LamdaCommand AddTransactions { get; set; }
        public LamdaCommand UpdateBalance { get; set; }
        public LamdaCommand BrowseCmd { get; set; }
        public AccountViewModel() : base() { }
        public AccountViewModel(AccountModel model) : base(model)
        {
        }

        protected override async void Initialize()
        {
            if (_m.Id == Guid.Empty)
                return;

            //setup commands
            AddTransactions = new LamdaCommand(
                (obj) => true,
                (obj) => ShowTransactionsDialog()
            );

            UpdateBalance = new LamdaCommand(
                (obj) => true,
                (obj) => ShowBalanceDialog()
            );

            BrowseCmd = new LamdaCommand(
                (obj) => true,
                (obj) => BrowseForFile()
            );

            Notify(nameof(AccountName));
            Notify(nameof(CurrencyType));

            //create list of all transactions
            List<TransactionModel> transactions = await SQLiteContext.GetAllAccountTransactionsAsync(_m.Id);
            foreach(TransactionModel transaction in transactions)
            {
                Transactions.Add(transaction);
            }

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

        private void ShowTransactionsDialog()
        {
            AddRawTransactionWindow transWin = new AddRawTransactionWindow(_m);
            transWin.ShowDialog();
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

        private async void BrowseForFile()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                RawTransFilePath = dialog.FileName;

                string fileText = File.ReadAllText(RawTransFilePath);
                string[] lines = fileText.Split('\n');

                var splitterRule = await SQLiteContext.GetConversionRule_Splitter(_m.Id);
                var nameRule = await SQLiteContext.GetConversionRule_Name(_m.Id);
                var dateRule = await SQLiteContext.GetConversionRule_Date(_m.Id);
                var dollarValueRule = await SQLiteContext.GetConversionRule_DollarValue(_m.Id);
                var categoryRule = await SQLiteContext.GetConversionRule_Category(_m.Id);
                var balanceRule = await SQLiteContext.GetConversionRule_Balance(_m.Id);

                ConvertTransactions.Clear();
                foreach (string line in lines)
                {
                    if (line == "")
                        continue;

                    ConvertTransactionViewModel ct = new ConvertTransactionViewModel(_m, line);
                    await ct.ConvertTransaction(splitterRule, nameRule, dateRule, dollarValueRule, balanceRule, categoryRule);
                    ConvertTransactions.Add(ct);
                }
            }
        }

    }
}
