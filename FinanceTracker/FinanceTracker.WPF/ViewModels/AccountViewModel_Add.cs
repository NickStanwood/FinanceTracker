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
        public LamdaCommand BrowseCmd { get; set; }

        private void Initialize_Add()
        {

            BrowseCmd = new LamdaCommand(
                (obj) => true,
                (obj) => BrowseForFile()
            );
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
