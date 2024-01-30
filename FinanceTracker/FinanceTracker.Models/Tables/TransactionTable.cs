using SQLite;

namespace FinanceTracker.Models
{
    internal class TransactionTable
    {
        public async Task<TransactionModel?> InsertTransaction(SQLiteAsyncConnection conn, Guid accId, DateTime date, double dollars, string name, Guid? categoryId)
        {
            TransactionModel transaction = new TransactionModel 
            { 
                Id = 0,
                AccountId = accId, 
                Date = date, 
                Name = name, 
                CategoryId = categoryId,
                DollarValue = dollars
            };

            int rowCount = await conn.InsertAsync(transaction);
            if (rowCount > 0)
                return transaction;
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
