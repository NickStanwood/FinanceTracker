using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SQLite;

namespace FinanceTracker.Models
{
    public static class SQLiteContext
    {
        private static SQLiteAsyncConnection _conn = null;
        private static AccountTable _accountTable = new AccountTable();
        private static TransactionTable _transactionTable = new TransactionTable();
        private static CategoryTable _categoryTable = new CategoryTable();
        private static CategoryRegexTable _categoryRegexTable = new CategoryRegexTable();

        private static ConversionRuleSplitterTable _conversionRuleSplitterTable = new ConversionRuleSplitterTable();
        private static ConversionRuleNameTable _conversionRuleNameTable = new ConversionRuleNameTable();
        private static ConversionRuleDateTable _conversionRuleDateTable = new ConversionRuleDateTable();
        private static ConversionRuleCategoryTable _conversionRuleCategoryTable = new ConversionRuleCategoryTable();
        private static ConversionRuleDollarValueTable _conversionRuleDollarValueTable = new ConversionRuleDollarValueTable();
        private static ConversionRuleBalanceTable _conversionRuleBalanceTable = new ConversionRuleBalanceTable();

        private static object _locker = new object();
        private static void Initialize()
        {
            if (_conn != null)
                return;

            lock(_locker)
            {
                if (_conn != null)
                    return;

                string connStr = ConfigurationManager.AppSettings["dbPath"];
                SQLiteAsyncConnection conn = new SQLiteAsyncConnection(connStr);
                conn.CreateTableAsync<AccountModel>().Wait();
                conn.CreateTableAsync<TransactionModel>().Wait();
                conn.CreateTableAsync<ConversionRuleNameModel>().Wait();
                conn.CreateTableAsync<ConversionRuleDateModel>().Wait();
                conn.CreateTableAsync<ConversionRuleCategoryModel>().Wait();
                conn.CreateTableAsync<ConversionRuleDollarValueModel>().Wait();
                conn.CreateTableAsync<ConversionRuleBalanceModel>().Wait();
                conn.CreateTableAsync<ConversionRuleSplitterModel>().Wait();
                conn.CreateTableAsync<CategoryModel>().Wait();
                conn.CreateTableAsync<CategoryRegexModel>().Wait();
                _conn = conn;

                List<CategoryModel> categories = GetAllCategories().Result;
                if (categories.Count == 0)
                {
                    CategoryModel inc = AddCategory(null, "Income").Result;
                    AddCategory(inc.Id, "Salary").Wait();
                    AddCategory(inc.Id, "Capital Gain").Wait();
                    CategoryModel exp = AddCategory(null, "Expense").Result;
                    CategoryModel trans = AddCategory(null, "Transfer").Result;
                    AddCategory(trans.Id, "Savings").Wait();
                    AddCategory(trans.Id, "Credit Card Bill").Wait();

                }
            }
                      
        }

        #region Account Table
        public async static Task<List<AccountModel>> GetAllAccountsAsync()
        {
            Initialize();
            return await _accountTable.SelectAll(_conn);
        }
        public async static Task<AccountModel?> AddAccount(string accName)
        {
            Initialize();
            return await _accountTable.InsertAccount(_conn, accName);
        }
        public async static Task UpdateAccount(AccountModel acc)
        {
            Initialize();
            await _accountTable.UpdateAccount(_conn, acc);
        }
        #endregion

        #region Transaction Table
        public async static Task<TransactionModel?> AddTransaction(TransactionModel transaction)
        {
            Initialize();
            return await _transactionTable.InsertTransaction(_conn, transaction);
        }
        public async static Task<int> AddTransactions(List<TransactionModel> transactions)
        {
            Initialize();
            return await _transactionTable.InsertTransactions(_conn, transactions);
        }

        public async static Task<List<TransactionModel>> GetAllAccountTransactionsAsync(Guid accId)
        {
            Initialize();
            return await _transactionTable.SelectAccountTransactions(_conn, accId);
        }

        public async static Task<List<TransactionModel>> GetAllAccountTransactionsAsync(Guid accId, DateTime since)
        {
            Initialize();
            return await _transactionTable.SelectTransactions(_conn, accId, since);
        }
        public async static Task<TransactionModel> GetAccountBalanceAsync(Guid accId)
        {
            Initialize();
            return await _transactionTable.SelectBalanceTransaction(_conn, accId);
        }
        #endregion

        #region Category Table
        public async static Task<CategoryModel> GetCategory(Guid categoryId)
        {
            Initialize();
            return await _categoryTable.Select(_conn, categoryId);
        }

        public async static Task<List<CategoryModel>> GetAllCategories()
        {
            Initialize();
            return await _categoryTable.SelectAll(_conn);
        }
        public async static Task<List<CategoryModel>> GetChildCategories(Guid? parentCategoryId)
        {
            Initialize();
            return await _categoryTable.SelectAllChildren(_conn, parentCategoryId);
        }
        
        public async static Task<CategoryModel?> AddCategory(Guid? parentCategory, string catName)
        {
            Initialize();
            return await _categoryTable.Insert(_conn, parentCategory, catName);
        }

        public async static Task UpdateCategoryt(CategoryModel cat)
        {
            Initialize();
            await _categoryTable.Update(_conn, cat);
        }
        public async static Task<List<CategoryRegexModel>> GetCategoryRegexes(Guid conversionRuleId)
        {
            Initialize();
            return await _categoryRegexTable.SelectConversionRuleRegexes(_conn, conversionRuleId);
        }

        public async static Task<CategoryRegexModel?> AddCategoryRegex(Guid conversionRuleId, Guid CategoryId, string regex)
        {
            Initialize();
            return await _categoryRegexTable.Insert(_conn, conversionRuleId, CategoryId, regex);
        }

        #endregion

        #region Conversion Tables
        public async static Task<ConversionRuleSplitterModel> GetConversionRule_Splitter(Guid accId)
        {
            Initialize();
            return await _conversionRuleSplitterTable.Select(_conn, accId);
        }

        public async static Task<ConversionRuleNameModel> GetConversionRule_Name(Guid accId)
        {
            Initialize();
            return await _conversionRuleNameTable.Select(_conn, accId);
        }

        public async static Task<ConversionRuleDateModel> GetConversionRule_Date(Guid accId)
        {
            Initialize();
            return await _conversionRuleDateTable.Select(_conn, accId);
        }

        public async static Task<ConversionRuleCategoryModel> GetConversionRule_Category(Guid accId)
        {
            Initialize();
            return await _conversionRuleCategoryTable.Select(_conn, accId);
        }

        public async static Task<ConversionRuleDollarValueModel> GetConversionRule_DollarValue(Guid accId)
        {
            Initialize();
            return await _conversionRuleDollarValueTable.Select(_conn, accId);
        }

        public async static Task<ConversionRuleBalanceModel> GetConversionRule_Balance(Guid accId)
        {
            Initialize();
            return await _conversionRuleBalanceTable.Select(_conn, accId);
        }

        public async static Task UpdateConversionRule_Splitter(ConversionRuleSplitterModel splitterRule)
        {
            Initialize();
            await _conversionRuleSplitterTable.Update(_conn, splitterRule);
        }
        public async static Task UpdateConversionRule_Name(ConversionRuleNameModel nameRule)
        {
            Initialize();
            await _conversionRuleNameTable.Update(_conn, nameRule);
        }
        public async static Task UpdateConversionRule_Date(ConversionRuleDateModel dateRule)
        {
            Initialize();
            await _conversionRuleDateTable.Update(_conn, dateRule);
        }
        public async static Task UpdateConversionRule_DollarValue(ConversionRuleDollarValueModel dollarValueRule)
        {
            Initialize();
            await _conversionRuleDollarValueTable.Update(_conn, dollarValueRule);
        }
        public async static Task UpdateConversionRule_Balance(ConversionRuleBalanceModel balanceRule)
        {
            Initialize();
            await _conversionRuleBalanceTable.Update(_conn, balanceRule);
        }
        public async static Task UpdateConversionRule_Category(ConversionRuleCategoryModel categoryRule)
        {
            Initialize();
            await _conversionRuleCategoryTable.Update(_conn, categoryRule);
            //TODO update regex list 
        }
        #endregion
    }
}
