﻿using System;
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
        private static ConversionRuleNameTable _conversionRuleNameTable = new ConversionRuleNameTable();
        private static async Task Initialize()
        {
            if (_conn != null)
                return;

            string connStr = ConfigurationManager.AppSettings["dbPath"];
            _conn = new SQLiteAsyncConnection(connStr);
            await _conn.CreateTableAsync<AccountModel>();
            await _conn.CreateTableAsync<TransactionModel>();
            await _conn.CreateTableAsync<CategoryModel>();
            await _conn.CreateTableAsync<ConversionRuleNameModel>();
        }

        #region Account Table
        public async static Task<List<AccountModel>> GetAllAccountsAsync()
        {
            await Initialize();
            return await _accountTable.SelectAll(_conn);
        }
        public async static Task<AccountModel?> AddAccount(string accName)
        {
            await Initialize();
            return await _accountTable.InsertAccount(_conn, accName);
        }
        public async static Task UpdateAccount(AccountModel acc)
        {
            await Initialize();
            await _accountTable.UpdateAccount(_conn, acc);
        }
        #endregion

        #region Transaction Table
        public async static Task<TransactionModel?> AddTransaction(Guid accId, DateTime date, double dollars, string name, Guid? categoryId)
        {
            await Initialize();
            return await _transactionTable.InsertTransaction(_conn, accId, date, dollars, name, categoryId);
        }

        public async static Task<List<TransactionModel>> GetAllAccountTransactionsAsync(Guid accId)
        {
            await Initialize();
            return await _transactionTable.SelectAccountTransactions(_conn, accId);
        }
        public async static Task<List<TransactionModel>> GetAllAccountTransactionsAsync(Guid accId, DateTime since)
        {
            await Initialize();
            return await _transactionTable.SelectTransactions(_conn, accId, since);
        }
        #endregion

        #region Category Table
        public async static Task<List<CategoryModel>> GetAllCategories()
        {
            await Initialize();
            return await _categoryTable.SelectAll(_conn);
        }
        #endregion

        #region Conversion Tables
        public async static Task<ConversionRuleNameModel> GetConversionRule_Name(Guid accId)
        {
            await Initialize();
            return await _conversionRuleNameTable.Select(_conn, accId);
        }
        #endregion
    }
}
