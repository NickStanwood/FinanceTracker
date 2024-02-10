using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FinanceTracker.Models
{
    internal class AccountTable
    {
        public async Task<AccountModel?> InsertAccount(SQLiteAsyncConnection conn, string accName)
        {
            AccountModel account = new AccountModel { Name = accName, Id = Guid.NewGuid(), CurrencyType = "CAD" };
            int rowCount = await conn.InsertAsync(account);
            if (rowCount > 0)
                return account;
            return null;
        }
        public async Task UpdateAccount(SQLiteAsyncConnection conn, AccountModel acc)
        {
            await conn.UpdateAsync(acc);
        }

        public async Task<AccountModel> SelectAccount(SQLiteAsyncConnection conn, Guid accId)
        {
            return await conn.Table<AccountModel>().Where(o => o.Id == accId).FirstOrDefaultAsync();
        }

        public async Task<List<AccountModel>> SelectAll(SQLiteAsyncConnection conn)
        {
            return await conn.Table<AccountModel>().ToListAsync();
        }
    }
}
