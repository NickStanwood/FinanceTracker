using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Models;

namespace FinanceTracker.WPF
{
    public class ConverterDeveloperViewModel : ViewModelBase<AccountModel>
    {
        private string _rawTransaction = "";
        public string RawTransaction { get { return _rawTransaction; } set { _rawTransaction = value; Notify(); } }

        public ObservableCollection<string> SplitTransaction { get; set; } = new ObservableCollection<string>();

        private string _name;
        public string Name { get { return _name; } set { _name = value; Notify(); } }

        private ConversionRuleNameModel _conversionRuleName;
        private ConversionRuleDateModel _conversionRuleDate;
        private ConversionRuleDollarValueModel _conversionRuleValue;
        private ConversionRuleCategoryModel _conversionRuleCategory;

        //private ConversionRuleBalance _conversionRuleBalance;
        //private ConversionRuleSplitter _conversionRuleSplitter;

        public ConverterDeveloperViewModel(string? rawTransaction, AccountModel model) : base(model)
        {
            if(rawTransaction != null)
                RawTransaction = rawTransaction;
        }

        protected async override void Initialize()
        {
            //_conversionRuleSplitter = await SQLiteContext.GetConversionRule_Splitter(_m.Id);
            //SplitTransaction = _conversionRuleSplitter.Convert(RawTransaction);
            SplitTransaction.Clear();
            string[] splitTrans = RawTransaction.Split('\t');
            foreach(string split in splitTrans)
            {
                if(split != "")
                    SplitTransaction.Add(split);
            }

            _conversionRuleName = await SQLiteContext.GetConversionRule_Name(_m.Id);
            Name = _conversionRuleName.Convert(SplitTransaction.ToList());
        }
    }
}
