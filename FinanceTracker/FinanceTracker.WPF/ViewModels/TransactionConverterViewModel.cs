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
        private string _rawTransFilePath;
        public string RawTransFilePath { get { return _rawTransFilePath; } set { _rawTransFilePath = value; Notify(); } }
        public ObservableCollection<RawTransactionModel> RawTransactions { get; set; } = new ObservableCollection<RawTransactionModel>();

        public ObservableCollection<TransactionModel> Transactions { get; set; } = new ObservableCollection<TransactionModel>();

        public LamdaCommand ConvertCmd { get; set; }
        public LamdaCommand BrowseCmd { get; set; }
        public LamdaCommand ViewRulesCmd { get; set; }
        public LamdaCommand SaveCmd { get; set; }
        public TransactionConverterViewModel(AccountModel model) : base(model) { }
        public TransactionConverterViewModel() : base() { }
        protected override void Initialize()
        {
            BrowseCmd = new LamdaCommand(
                (obj) => true,
                (obj) => BrowseForFile()
            );

            ConvertCmd = new LamdaCommand(
                (obj) => true,
                (obj) => ConvertTransactions()
            );

            ViewRulesCmd = new LamdaCommand(
                (obj) => true,
                (obj) => ViewConversions()
            );

            SaveCmd = new LamdaCommand(
                (obj) => true,
                (obj) => SaveTransactions()
            );
        }

        private void ConvertCmd_CanExecuteChanged(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ConvertTransactions()
        {

        }

        private void BrowseForFile()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                RawTransFilePath = dialog.FileName;

                string fileText = File.ReadAllText(RawTransFilePath);
                string[] lines = fileText.Split('\n');

                RawTransactions.Clear();
                foreach (string line in lines)
                {
                    RawTransactionModel model = new RawTransactionModel { RawTransaction = line};
                    RawTransactions.Add(model);
                }
            }
        }

        private void ViewConversions()
        {

        }

        private void SaveTransactions()
        {

        }
    }
}
