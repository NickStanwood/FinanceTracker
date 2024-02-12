using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FinanceTracker.Models;

namespace FinanceTracker.WPF
{
    internal class TransactionConverterViewModel : ViewModelBase<AccountModel>
    {

        public ObservableCollection<TransactionModel> Transactions { get; set; } = new ObservableCollection<TransactionModel>();

        public LamdaCommand ConvertCmd { get; set; }
        public LamdaCommand ViewRulesCmd { get; set; }
        public LamdaCommand SaveCmd { get; set; }
        public TransactionConverterViewModel(AccountModel model) : base(model) { }
        public TransactionConverterViewModel() : base() { }
        protected override void Initialize()
        {
            ConvertCmd = new LamdaCommand(
                (obj) => true,
                async (obj) => await ConvertTransactions()
            );

            ViewRulesCmd = new LamdaCommand(
                (obj) => true,
                (obj) => ViewConversions()
            );

            SaveCmd = new LamdaCommand(
                (obj) => true,
                async (obj) => await SaveTransactions()
            );
        }

        private async Task ConvertTransactions()
        {
            var splitterRule = await SQLiteContext.GetConversionRule_Splitter(_m.Id);
            var nameRule = await SQLiteContext.GetConversionRule_Name(_m.Id);
            var dateRule = await SQLiteContext.GetConversionRule_Date(_m.Id);
            var dollarValueRule = await SQLiteContext.GetConversionRule_DollarValue(_m.Id);
            var categoryRule = await SQLiteContext.GetConversionRule_Category(_m.Id);
            var balanceRule = await SQLiteContext.GetConversionRule_Balance(_m.Id);

            Transactions.Clear();
            //foreach (RawTransactionModel trans in RawTransactions)
            //{
            //    try
            //    {
            //        List<string> splitTrans = splitterRule.Convert(trans.RawTransaction).ToList();
            //        string name = nameRule.Convert(splitTrans);
            //        DateTime date = dateRule.Convert(splitTrans);
            //        double dollarValue = dollarValueRule.Convert(splitTrans);
            //        double? balance = balanceRule.TryConvert(splitTrans);
            //        CategoryModel? category = await categoryRule.TryConvert(splitTrans);

            //        TransactionModel tm = new TransactionModel
            //        {
            //            AccountId = _m.Id,
            //            Name = name,
            //            Date = date,
            //            DollarValue = dollarValue,
            //            Balance = balance,
            //            CategoryId = category?.Id,
            //        };
            //        Transactions.Add(tm);
            //    }
            //    catch (Exception ex)
            //    {
            //        continue;
            //    }
            //}

        }


        private void ViewConversions()
        {
            //ConverterDeveloperViewModel convVm = new ConverterDeveloperViewModel(RawTransactions.FirstOrDefault()?.RawTransaction, _m);
            //ConverterDeveloperWindow converterWindow = new ConverterDeveloperWindow(convVm);
            //converterWindow.ShowDialog();
        }

        private async Task SaveTransactions()
        {
            foreach(TransactionModel trans in Transactions)
            {
                TransactionModel? tm = await SQLiteContext.AddTransaction(trans);
            }

            Transactions.Clear();
        }
    }
}
