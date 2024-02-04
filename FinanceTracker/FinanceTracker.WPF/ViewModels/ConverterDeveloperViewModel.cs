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

        private DateTime _date;
        public DateTime Date{ get { return _date; } set { _date = value; Notify(); } }

        private double _dollarValue;
        public double DollarValue { get { return _dollarValue; } set { _dollarValue = value; Notify(); } }

        private double _balance;
        public double Balance { get { return _balance; } set { _balance = value; Notify(); } }

        private CategoryModel _category;
        public CategoryModel Category { get { return _category; } set { _category = value; Notify(); } }

        private ConversionRuleNameModel _conversionRuleName;
        private ConversionRuleDateModel _conversionRuleDate;
        private ConversionRuleDollarValueModel _conversionRuleValue;
        private ConversionRuleCategoryModel _conversionRuleCategory;

        private ConversionRuleBalanceModel _conversionRuleBalance;
        private ConversionRuleSplitterModel _conversionRuleSplitter;

        public ConverterDeveloperViewModel(string? rawTransaction, AccountModel model) : base(model)
        {
            if(rawTransaction != null)
                RawTransaction = rawTransaction;
        }

        protected async override void Initialize()
        {
            _conversionRuleSplitter = await SQLiteContext.GetConversionRule_Splitter(_m.Id);
            _conversionRuleName = await SQLiteContext.GetConversionRule_Name(_m.Id);
            _conversionRuleDate = await SQLiteContext.GetConversionRule_Date(_m.Id);
            _conversionRuleValue = await SQLiteContext.GetConversionRule_DollarValue(_m.Id);
            _conversionRuleCategory = await SQLiteContext.GetConversionRule_Category(_m.Id);
            _conversionRuleBalance = await SQLiteContext.GetConversionRule_Balance(_m.Id);

            SplitTransaction.Clear();
            string[] splitTrans = _conversionRuleSplitter.Convert(RawTransaction);
            foreach (string split in splitTrans)
            {
                if(split != "")
                    SplitTransaction.Add(split);
            }


            Name = _conversionRuleName.Convert(SplitTransaction.ToList());
            Date = _conversionRuleDate.Convert(SplitTransaction.ToList());
            DollarValue = _conversionRuleValue.Convert(SplitTransaction.ToList());
            Balance = _conversionRuleBalance.Convert(SplitTransaction.ToList());
            Category = await _conversionRuleCategory.Convert(SplitTransaction.ToList());

        }
    }
}
