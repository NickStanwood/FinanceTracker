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
    //Add Transaction Tab of AccountViewModel
    public partial class AccountViewModel
    {
        private string _rawTransFilePath;
        public string RawTransFilePath { get { return _rawTransFilePath; } set { _rawTransFilePath = value; Notify(); } }
        public ObservableCollection<ConvertTransactionViewModel> ConvertTransactions { get; set; } = new ObservableCollection<ConvertTransactionViewModel>();
        private ConvertTransactionViewModel _selecectedConvertedTransaction;
        public ConvertTransactionViewModel SelectedConvertedTransaction { get { return _selecectedConvertedTransaction; } set { _selecectedConvertedTransaction = value; Notify(); } }
        public LamdaCommand BrowseCmd { get; set; }
        public LamdaCommand RefreshCmd { get; set; }
        public LamdaCommand SaveCmd { get; set; }

        //spliter rule 
        public string SplitterRule_DelimChar { get { return splitterRule_ != null ? splitterRule_.DelimChar : ""; } set { splitterRule_.DelimChar = value; Notify(); } }
        public bool SplitterRule_IgnoreDelimInQuotes { get { return splitterRule_ != null ? splitterRule_.IgnoreDelimInQuotes : false; } set { splitterRule_.IgnoreDelimInQuotes = value; Notify(); } }

        //Name Rule
        public int NameRule_Column { get { return nameRule_ != null ? nameRule_.Column : 0; } set { nameRule_.Column = value; Notify(); } }

        //Date Rule
        public int DateRule_Column { get { return dateRule_ != null ? dateRule_.Column : 0; } set { dateRule_.Column = value; Notify(); } }
        public string DateRule_DateFormat { get { return dateRule_ != null ? dateRule_.DateFormat : ""; } set { dateRule_.DateFormat = value; Notify(); } }

        //Dollar Value Rule
        public int DollarValueRule_Column { get { return dollarValueRule_ != null ? dollarValueRule_.Column : 0; } set { dollarValueRule_.Column = value; Notify(); } }
        public bool DollarValueRule_ApplyNegation { get { return dollarValueRule_ != null ? dollarValueRule_.ApplyNegation : false; } set { dollarValueRule_.ApplyNegation = value; Notify(); } }


        //Balance Rule
        public int BalanceRule_Column { get { return balanceRule_ != null ? balanceRule_.Column : 0; } set { balanceRule_.Column = value; Notify(); } }
        public bool BalanceRule_ApplyNegation { get { return balanceRule_ != null ? balanceRule_.ApplyNegation : false; } set { balanceRule_.ApplyNegation = value; Notify(); } }
        public bool BalanceRule_DataAvailable { get { return balanceRule_ != null ? balanceRule_.DataAvailable : false; } set { balanceRule_.DataAvailable = value; Notify(); } }

        //conversion rules
        private ConversionRuleSplitterModel splitterRule_;
        private ConversionRuleNameModel nameRule_;
        private ConversionRuleDateModel dateRule_;
        private ConversionRuleDollarValueModel dollarValueRule_;
        private ConversionRuleBalanceModel balanceRule_;
        private ConversionRuleCategoryModel categoryRule_;

        private async void Initialize_Add()
        {
            BrowseCmd = new LamdaCommand(
                (obj) => true,
                (obj) => BrowseForFile()
            );

            RefreshCmd = new LamdaCommand(
                (obj) => true,
                (obj) => ConvertRawTransactions()
            );

            SaveCmd = new LamdaCommand(
                (obj) => true,
                (obj) => SaveConvertedTransactions()
            );

            splitterRule_ = await SQLiteContext.GetConversionRule_Splitter(_m.Id);
            nameRule_ = await SQLiteContext.GetConversionRule_Name(_m.Id);
            dateRule_ = await SQLiteContext.GetConversionRule_Date(_m.Id);
            dollarValueRule_ = await SQLiteContext.GetConversionRule_DollarValue(_m.Id);
            balanceRule_ = await SQLiteContext.GetConversionRule_Balance(_m.Id);
            categoryRule_ = await SQLiteContext.GetConversionRule_Category(_m.Id);
            Notify(nameof(SplitterRule_DelimChar));
            Notify(nameof(SplitterRule_IgnoreDelimInQuotes));
            Notify(nameof(NameRule_Column));
            Notify(nameof(DateRule_Column));
            Notify(nameof(DateRule_DateFormat));
            Notify(nameof(DollarValueRule_Column));
            Notify(nameof(DollarValueRule_ApplyNegation));
            Notify(nameof(BalanceRule_Column));
            Notify(nameof(BalanceRule_ApplyNegation));
            Notify(nameof(BalanceRule_DataAvailable));
        }

        private async Task BrowseForFile()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                RawTransFilePath = dialog.FileName;

                await ConvertRawTransactions();
            }
        }

        private async Task ConvertRawTransactions()
        {
            string fileText = File.ReadAllText(RawTransFilePath);
            string[] lines = fileText.Split('\n');

            ConvertTransactions.Clear();
            foreach (string line in lines)
            {
                if (line == "")
                    continue;

                ConvertTransactionViewModel ct = new ConvertTransactionViewModel(_m, line);
                await ct.ConvertTransaction(splitterRule_, nameRule_, dateRule_, dollarValueRule_, balanceRule_, categoryRule_);
                ConvertTransactions.Add(ct);
            }
        }

        private async Task SaveConvertedTransactions()
        {
            await SQLiteContext.UpdateConversionRule_Splitter(splitterRule_);
            await SQLiteContext.UpdateConversionRule_Name(nameRule_);
            await SQLiteContext.UpdateConversionRule_Date(dateRule_);
            await SQLiteContext.UpdateConversionRule_DollarValue(dollarValueRule_);
            await SQLiteContext.UpdateConversionRule_Balance(balanceRule_);
            await SQLiteContext.UpdateConversionRule_Category(categoryRule_);

            List<TransactionModel> convTrans = new List<TransactionModel>();
            foreach(ConvertTransactionViewModel ct in ConvertTransactions)
            {
                if(ct.ConvertedTrans != null)
                    convTrans.Add(ct.ConvertedTrans);
            }

            await SQLiteContext.AddTransactions(convTrans);

            ConvertTransactions.Clear();
        }
    }
}
