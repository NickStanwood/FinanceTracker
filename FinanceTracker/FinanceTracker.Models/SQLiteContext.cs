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
        private static async Task Initialize()
        {
            if (_conn != null)
                return;

            string connStr = ConfigurationManager.AppSettings["dbPath"];
            _conn = new SQLiteAsyncConnection(connStr);
            await _conn.CreateTableAsync<AccountModel>();
            await _conn.CreateTableAsync<TransactionModel>();
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
        #endregion


        #region Transaction Table
        public async static Task<TransactionModel?> AddTransaction(Guid accId, DateTime date, string name, string category, double dollars)
        {
            await Initialize();
            return await _transactionTable.InsertTransaction(_conn, accId, date, name, category, dollars);
        }

        public async static Task<List<TransactionModel>> GetAllAccountTransactionsAsync(Guid accId)
        {
            await Initialize();
            return await _transactionTable.SelectTransactions(_conn, accId);
        }
        #endregion
    }
}
