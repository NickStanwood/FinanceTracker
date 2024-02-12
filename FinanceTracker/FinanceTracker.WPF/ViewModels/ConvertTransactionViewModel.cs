﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Models;

namespace FinanceTracker.WPF
{
    public class ConvertTransactionViewModel : ViewModelBase<AccountModel>
    {
        public string RawTransaction 
        { 
            get 
            { 
                return _rawTrans.RawTransaction; 
            } 
            set 
            { 
                _rawTrans.RawTransaction = value; 
                Notify(); 
            } 
        }

        public string? ConvName 
        { 
            get 
            { 
                return _convertedTrans?.Name; 
            } 
            set 
            { 
                if(_convertedTrans != null && value != null)
                    _convertedTrans.Name = value; 
                Notify(); 
            } 
        }

        public DateTime? ConvDate 
        { 
            get 
            { 
                return _convertedTrans?.Date; 
            } 
            set
            {
                if (_convertedTrans != null && value != null)
                    _convertedTrans.Date = (DateTime)value; 
                Notify(); 
            } 
        }

        public double? ConvDollarValue 
        { 
            get 
            {
                return _convertedTrans?.DollarValue; 
            } 
            set
            {
                if (_convertedTrans != null && value != null)
                    _convertedTrans.DollarValue = (double)value; 
                Notify(); 
            } 
        }

        public double? ConvBalance 
        { 
            get 
            { 
                return _convertedTrans?.Balance; 
            } 
            set
            {
                if (_convertedTrans != null && value != null)
                    _convertedTrans.Balance = value; 
                Notify(); 
            } 
        }

        public ObservableCollection<string> ConversionErrors = new ObservableCollection<string>();

        private RawTransactionModel _rawTrans { get; set; }
        private TransactionModel? _convertedTrans { get; set; }



        public ConvertTransactionViewModel(AccountModel acc, string rawTrans) : base(acc)
        {
            _rawTrans = new RawTransactionModel
            {
                RawTransaction = rawTrans,
                AccountId = _m.Id
            };
        }

        protected override void Initialize()
        {
        }

        public async Task ConvertTransaction(ConversionRuleSplitterModel splitterRule,
            ConversionRuleNameModel nameRule,
            ConversionRuleDateModel dateRule,
            ConversionRuleDollarValueModel dollarValueRule,
            ConversionRuleBalanceModel balanceRule,
            ConversionRuleCategoryModel categoryRule)
        {
            ConversionErrors.Clear();
            List<string> splitTrans = null;
            string name = "";
            DateTime date = DateTime.MinValue;
            double dollarValue = 0.0;
            double? balance = null;
            CategoryModel? category = null;
            try
            {
                splitTrans = splitterRule.Convert(RawTransaction).ToList();
            }
            catch (Exception ex)
            {
                ConversionErrors.Add("[Split] " + ex.Message);
                return;
            }

            try
            {
                name = nameRule.Convert(splitTrans);
            }
            catch (Exception ex)
            {
                ConversionErrors.Add("[ Name     ] " + ex.Message);
            }

            try
            {
                date = dateRule.Convert(splitTrans);
            }
            catch (Exception ex)
            {
                ConversionErrors.Add("[ Date     ] " + ex.Message);
            }

            try
            {
                dollarValue = dollarValueRule.Convert(splitTrans);
            }
            catch (Exception ex)
            {
                ConversionErrors.Add("[ Value    ] " + ex.Message);
            }

            try
            {
                balance = balanceRule.Convert(splitTrans);
            }
            catch (Exception ex)
            {
                ConversionErrors.Add("[ Balance  ] " + ex.Message);
            }

            try
            {
                category = await categoryRule.Convert(splitTrans);
            }
            catch (Exception ex)
            {
                ConversionErrors.Add("[ Category ] " + ex.Message);
            }

            _convertedTrans = new TransactionModel
            {
                AccountId = _m.Id,
                Name = name,
                Date = date,
                DollarValue = dollarValue,
                Balance = balance,
                CategoryId = category?.Id,
            };
        }
    }
}
