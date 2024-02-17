using SQLite;

namespace FinanceTracker.Models
{
    internal class TransactionTable
    {
        public async Task<TransactionModel?> InsertTransaction(SQLiteAsyncConnection conn, TransactionModel tm)
        {
            int rowCount = await conn.InsertAsync(tm);
            if (rowCount > 0)
                return tm;
            return null;
        }
        public async Task<int> InsertTransactions(SQLiteAsyncConnection conn, List<TransactionModel> tml)
        {
            int rowCount = await conn.InsertAllAsync(tml);
            return rowCount;
        }
        public async Task<TransactionModel> SelectBalanceTransaction(SQLiteAsyncConnection conn, Guid accId)
        {
            return await conn.Table<TransactionModel>().Where(o => o.AccountId == accId && o.Balance != null).OrderByDescending(o => o.Date).FirstOrDefaultAsync();
        }

        public async Task<List<TransactionModel>> SelectAccountTransactions(SQLiteAsyncConnection conn, Guid accId)
        {
            return await conn.Table<TransactionModel>().Where(o => o.AccountId == accId).OrderByDescending(o => o.Date).ToListAsync();
        }

        public async Task<List<TransactionModel>> SelectTransactions(SQLiteAsyncConnection conn, Guid accId, DateTime since)
        {
            return await conn.Table<TransactionModel>().Where(o => o.AccountId == accId && o.Date > since).OrderByDescending(o => o.Date).ToListAsync();
        }

        public async Task<List<TransactionModel>> SelectCategoryTransactions(SQLiteAsyncConnection conn, Guid categoryId)
        {
            return await conn.Table<TransactionModel>().Where(o => o.CategoryId == categoryId).ToListAsync();
        }

        public async Task<List<AccountModel>> SelectAll(SQLiteAsyncConnection conn)
        {
            return await conn.Table<AccountModel>().ToListAsync();
        }
    }
}
