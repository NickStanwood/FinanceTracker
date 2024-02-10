using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using FinanceTracker.Models;

namespace FinanceTracker.WPF
{
    enum ActiveDevelopment
    {
        Splitter,
        Name,
        Date,
        DollarValue,
        Balance,
        Category
    }


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

        private string _output = "";
        public string Output { get { return _output; } set { _output = value; Notify(); } }


        //Conversion Development Properties
        private string _conversionRuleTitle;
        public string ConversionRuleTitle { get { return _conversionRuleTitle; } set { _conversionRuleTitle = value; Notify(); } }

        private int _column;
        public int Column { get { return _column; } set { _column = value; Notify(); } }
        
        private string _delimChar;
        public string DelimChar { get { return _delimChar; } set { _delimChar = value; Notify(); } }

        private string _dateFormat;
        public string DateFormat { get { return _dateFormat; } set { _dateFormat = value; Notify(); } }

        private bool _applyNegation;
        public bool ApplyNegation { get { return _applyNegation; } set { _applyNegation = value; Notify(); } }

        private bool _ignoreDelimInQuotes;
        public bool IgnoreDelimInQuotes { get { return _ignoreDelimInQuotes; } set { _ignoreDelimInQuotes = value; Notify(); } }

        private bool _dataAvailable;
        public bool DataAvailable { get { return _dataAvailable; } set { _dataAvailable = value; Notify(); } }


        //Conversion Development Visibility Properties
        private Visibility _columnVisible = Visibility.Collapsed;
        public Visibility ColumnVisible { get { return _columnVisible; } set { _columnVisible = value; Notify(); } }

        private Visibility _delimCharVisible = Visibility.Collapsed;
        public Visibility DelimCharVisible { get { return _delimCharVisible; } set { _delimCharVisible = value; Notify(); } }

        private Visibility _dateFormatVisible = Visibility.Collapsed;
        public Visibility DateFormatVisible { get { return _dateFormatVisible; } set { _dateFormatVisible = value; Notify(); } }

        private Visibility _applyNegationVisible = Visibility.Collapsed;
        public Visibility ApplyNegationVisible { get { return _applyNegationVisible; } set { _applyNegationVisible = value; Notify(); } }

        private Visibility _ignoreDelimInQuotesVisible = Visibility.Collapsed;
        public Visibility IgnoreDelimInQuotesVisible { get { return _ignoreDelimInQuotesVisible; } set { _ignoreDelimInQuotesVisible = value; Notify(); } }

        private Visibility _additiveUpdateVisible = Visibility.Collapsed;
        public Visibility AdditiveUpdateVisible { get { return _additiveUpdateVisible; } set { _additiveUpdateVisible = value; Notify(); } }

        public LamdaCommand ConvertCommand { get; set; }
        public LamdaCommand ViewCommand { get; set; }
        public LamdaCommand SaveCommand { get; set; }

        private ConversionRuleNameModel _conversionRuleName;
        private ConversionRuleDateModel _conversionRuleDate;
        private ConversionRuleDollarValueModel _conversionRuleValue;
        private ConversionRuleCategoryModel _conversionRuleCategory;

        private ConversionRuleBalanceModel _conversionRuleBalance;
        private ConversionRuleSplitterModel _conversionRuleSplitter;

        private ActiveDevelopment _activeDevelopment;

        public ConverterDeveloperViewModel(string? rawTransaction, AccountModel model) : base(model)
        {
            if(rawTransaction != null)
                RawTransaction = rawTransaction;

            ConvertCommand = new LamdaCommand(
                (obj) => true,
                async (obj) => await Convert()
            );

            ViewCommand = new LamdaCommand(
                (obj) => true,
                (obj) => ViewDevelopment(obj)
            ); 
            SaveCommand = new LamdaCommand(
                 (obj) => true,
                 (obj) => SaveChanges()
             );
        }

        protected async override void Initialize()
        {
            _conversionRuleSplitter = await SQLiteContext.GetConversionRule_Splitter(_m.Id);
            _conversionRuleName = await SQLiteContext.GetConversionRule_Name(_m.Id);
            _conversionRuleDate = await SQLiteContext.GetConversionRule_Date(_m.Id);
            _conversionRuleValue = await SQLiteContext.GetConversionRule_DollarValue(_m.Id);
            _conversionRuleCategory = await SQLiteContext.GetConversionRule_Category(_m.Id);
            _conversionRuleBalance = await SQLiteContext.GetConversionRule_Balance(_m.Id);

            await Convert();
        }

        private async Task Convert()
        {
            Output = "";
            try
            {

                SplitTransaction.Clear(); 
                string[] splitTrans = _conversionRuleSplitter.Convert(RawTransaction);
                foreach (string split in splitTrans)
                {
                    SplitTransaction.Add(split);
                }

            }
            catch (Exception ex)
            {
                Output += "[Split] " + ex.Message + "\n";
                return;
            }

            try
            {               
                Name = _conversionRuleName.Convert(SplitTransaction.ToList());
            }
            catch (Exception ex)
            {
                Output += "[ Name     ] " + ex.Message + "\n";
            }

            try
            {
                Date = _conversionRuleDate.Convert(SplitTransaction.ToList());
            }
            catch (Exception ex)
            {
                Output += "[ Date     ] " + ex.Message + "\n";
            }

            try
            {
                DollarValue = _conversionRuleValue.Convert(SplitTransaction.ToList());
            }
            catch (Exception ex)
            {
                Output += "[ Value    ] " + ex.Message + "\n";
            }

            try
            {
                Balance = _conversionRuleBalance.Convert(SplitTransaction.ToList());
            }
            catch (Exception ex)
            {
                Output += "[ Balance  ] " + ex.Message + "\n";
            }

            try
            {
                Category = await _conversionRuleCategory.Convert(SplitTransaction.ToList());
            }
            catch (Exception ex)
            {
                Output += "[ Category ] " + ex.Message + "\n";
            }
        }

        private void ViewDevelopment(object? parameter)
        {
            string? rule = parameter as string;
            if (rule == null)
                return;

            switch (rule)
            {
                case "splitter":
                    ConversionRuleTitle = "Split Transaction into Segments";
                    _activeDevelopment = ActiveDevelopment.Splitter;

                    DelimChar = _conversionRuleSplitter.DelimChar;
                    IgnoreDelimInQuotes = _conversionRuleSplitter.IgnoreDelimInQuotes;

                    ColumnVisible               = Visibility.Collapsed;
                    DelimCharVisible            = Visibility.Visible;
                    DateFormatVisible           = Visibility.Collapsed;
                    ApplyNegationVisible        = Visibility.Collapsed;
                    AdditiveUpdateVisible       = Visibility.Collapsed;
                    IgnoreDelimInQuotesVisible  = Visibility.Visible;
                    break;
                case "name":
                    ConversionRuleTitle = "Transaction Name Conversion";
                    _activeDevelopment = ActiveDevelopment.Name;

                    Column = _conversionRuleName.Column;
                    
                    ColumnVisible               = Visibility.Visible;
                    DelimCharVisible            = Visibility.Collapsed;
                    DateFormatVisible           = Visibility.Collapsed;
                    ApplyNegationVisible        = Visibility.Collapsed;
                    AdditiveUpdateVisible       = Visibility.Collapsed;
                    IgnoreDelimInQuotesVisible  = Visibility.Collapsed;
                    break;
                case "date":
                    ConversionRuleTitle = "Transaction Date Conversion";
                    _activeDevelopment = ActiveDevelopment.Date;

                    Column = _conversionRuleDate.Column;
                    DateFormat = _conversionRuleDate.DateFormat;

                    ColumnVisible               = Visibility.Visible;
                    DelimCharVisible            = Visibility.Collapsed;
                    DateFormatVisible           = Visibility.Visible;
                    ApplyNegationVisible        = Visibility.Collapsed;
                    AdditiveUpdateVisible       = Visibility.Collapsed;
                    IgnoreDelimInQuotesVisible  = Visibility.Collapsed;
                    break;
                case "dollarValue":
                    ConversionRuleTitle = "Transaction Dollar Value Conversion";
                    _activeDevelopment = ActiveDevelopment.DollarValue;

                    Column = _conversionRuleValue.Column;
                    ApplyNegation = _conversionRuleValue.ApplyNegation;
                    
                    ColumnVisible               = Visibility.Visible;
                    DelimCharVisible            = Visibility.Collapsed;
                    DateFormatVisible           = Visibility.Collapsed;
                    ApplyNegationVisible        = Visibility.Visible;
                    AdditiveUpdateVisible       = Visibility.Collapsed;
                    IgnoreDelimInQuotesVisible  = Visibility.Collapsed;
                    break;
                case "balance":
                    ConversionRuleTitle = "Transaction Account Balance Update Conversion";
                    _activeDevelopment = ActiveDevelopment.Balance;

                    Column = _conversionRuleBalance.Column;
                    ApplyNegation = _conversionRuleBalance.ApplyNegation;
                    DataAvailable = _conversionRuleBalance.DataAvailable;
                    
                    ColumnVisible               = Visibility.Visible;
                    DelimCharVisible            = Visibility.Collapsed;
                    DateFormatVisible           = Visibility.Collapsed;
                    ApplyNegationVisible        = Visibility.Visible;
                    AdditiveUpdateVisible       = Visibility.Visible;
                    IgnoreDelimInQuotesVisible  = Visibility.Collapsed;
                    break;
                case "category":
                    ConversionRuleTitle = "Transaction Category Conversion";
                    _activeDevelopment = ActiveDevelopment.Category;

                    Column = _conversionRuleBalance.Column;
                    //TODO add regex list

                    ColumnVisible               = Visibility.Visible;
                    DelimCharVisible            = Visibility.Collapsed;
                    DateFormatVisible           = Visibility.Collapsed;
                    ApplyNegationVisible        = Visibility.Collapsed;
                    AdditiveUpdateVisible       = Visibility.Collapsed;
                    IgnoreDelimInQuotesVisible  = Visibility.Collapsed;
                    break;
            }                 
        }

        private async void SaveChanges()
        {
            switch (_activeDevelopment)
            {
                case ActiveDevelopment.Splitter:
                    _conversionRuleSplitter.DelimChar = DelimChar;
                    _conversionRuleSplitter.IgnoreDelimInQuotes = IgnoreDelimInQuotes;
                    await SQLiteContext.UpdateConversionRule_Splitter(_conversionRuleSplitter);
                    break;
                case ActiveDevelopment.Name:
                    _conversionRuleName.Column = Column;
                    await SQLiteContext.UpdateConversionRule_Name(_conversionRuleName);
                    break;
                case ActiveDevelopment.Date:
                    _conversionRuleDate.Column = Column;
                    _conversionRuleDate.DateFormat = DateFormat;
                    await SQLiteContext.UpdateConversionRule_Date(_conversionRuleDate);
                    break;
                case ActiveDevelopment.DollarValue:
                    _conversionRuleValue.Column = Column;
                    _conversionRuleValue.ApplyNegation = ApplyNegation;
                    await SQLiteContext.UpdateConversionRule_DollarValue(_conversionRuleValue);
                    break;
                case ActiveDevelopment.Balance:
                    _conversionRuleBalance.Column = Column;
                    _conversionRuleBalance.ApplyNegation = ApplyNegation;
                    _conversionRuleBalance.DataAvailable = DataAvailable;
                    await SQLiteContext.UpdateConversionRule_Balance(_conversionRuleBalance);
                    break;
                case ActiveDevelopment.Category:
                    _conversionRuleCategory.Column = Column;
                    //TODO update regex list 
                    await SQLiteContext.UpdateConversionRule_Category(_conversionRuleCategory);
                    break;
            }
        }
    }
}
