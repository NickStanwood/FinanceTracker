using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FinanceTracker.Models
{
    internal class ConversionRuleCategoryTable
    {
        public async Task<ConversionRuleCategoryModel> Select(SQLiteAsyncConnection conn, Guid accId)
        {
            ConversionRuleCategoryModel conv = await conn.Table<ConversionRuleCategoryModel>().Where((o) => o.AccountId == accId).FirstOrDefaultAsync();
            if (conv == null)
                return await InsertDefault(conn, accId);

            return conv;
        }

        public async Task Update(SQLiteAsyncConnection conn, ConversionRuleCategoryModel categoryRule)
        {
            await conn.UpdateAsync(categoryRule);
        }

        private async Task<ConversionRuleCategoryModel> InsertDefault(SQLiteAsyncConnection conn, Guid accId)
        {
            ConversionRuleCategoryModel conv = new ConversionRuleCategoryModel
            {
                Id = Guid.NewGuid(),
                AccountId = accId,
                Column = 0,
                UseAdvanced = false,
                AdvancedScript = "function GetCategory(splitTrans){\n\n}"
            };

            int rowCount = await conn.InsertAsync(conv);
            if (rowCount <= 0)
                throw new Exception("Error inserting ConversionRuleCategoryModel");

            return conv;
        }
    }
}
