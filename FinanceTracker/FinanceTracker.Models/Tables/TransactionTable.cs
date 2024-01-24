using SQLite;

namespace FinanceTracker.Models
{
    internal class TransactionTable
    {
        public async Task<TransactionModel?> InsertTransaction(SQLiteAsyncConnection conn, Guid accId, DateTime date, string name, string category, double dollars)
        {
            TransactionModel transaction = new TransactionModel 
            { 
                Id = 0,
                AccountId = accId, 
                Date = date, 
                Name = name, 
                Category = category,
                DollarValue = dollars
            };

            int rowCount = await conn.InsertAsync(transaction);
            if (rowCount > 0)
                return transaction;
            return null;
        }
        public async Task<List<TransactionModel>> SelectTransactions(SQLiteAsyncConnection conn, Guid accId)
        {
            return await conn.Table<TransactionModel>().Where(o => o.AccountId == accId).ToListAsync();
        }

        public async Task<List<TransactionModel>> SelectTransactions(SQLiteAsyncConnection conn, string category)
        {
            return await conn.Table<TransactionModel>().Where(o => o.Category == category).ToListAsync();
        }

        public async Task<List<AccountModel>> SelectAll(SQLiteAsyncConnection conn)
        {
            return await conn.Table<AccountModel>().ToListAsync();
        }
    }
}
