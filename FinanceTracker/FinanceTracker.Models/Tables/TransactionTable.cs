using SQLite;

namespace FinanceTracker.Models
{
    internal class TransactionTable
    {
        public async Task<TransactionModel?> InsertTransaction(SQLiteAsyncConnection conn, TransactionModel tm)
        {
            TransactionModel duplicate = await conn.Table<TransactionModel>()
                .Where(o => 
                    o.AccountId == tm.AccountId
                    && o.DollarValue == tm.DollarValue
                    && o.Balance == tm.Balance
                    && o.Date == tm.Date
                    && o.Name == tm.Name).FirstOrDefaultAsync();

            //this transaction already exists in the database
            if(duplicate != null)
                return duplicate;

            int rowCount = await conn.InsertAsync(tm);
            if (rowCount > 0)
                return tm;
            return null;
        }
        public async Task<List<TransactionModel>> SelectAccountTransactions(SQLiteAsyncConnection conn, Guid accId)
        {
            return await conn.Table<TransactionModel>().Where(o => o.AccountId == accId).ToListAsync();
        }
        public async Task<List<TransactionModel>> SelectTransactions(SQLiteAsyncConnection conn, Guid accId, DateTime since)
        {
            return await conn.Table<TransactionModel>().Where(o => o.AccountId == accId && o.Date > since).ToListAsync();
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
