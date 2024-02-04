using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FinanceTracker.Models
{
    internal class ConversionRuleDollarValueTable
    {
        public async Task<ConversionRuleDollarValueModel> Select(SQLiteAsyncConnection conn, Guid accId)
        {
            ConversionRuleDollarValueModel conv = await conn.Table<ConversionRuleDollarValueModel>().Where((o) => o.AccountId == accId).FirstOrDefaultAsync();
            if (conv == null)
                return await InsertDefault(conn, accId);

            return conv;
        }

        public async Task<ConversionRuleDollarValueModel> InsertDefault(SQLiteAsyncConnection conn, Guid accId)
        {
            ConversionRuleDollarValueModel conv = new ConversionRuleDollarValueModel
            {
                Id = Guid.NewGuid(),
                AccountId = accId,
                Column = 0,
                ApplyNegation = false,
                UseAdvanced = false,
                AdvancedScript = "function GetDollarValue(splitTrans){\n\n}"
            };

            int rowCount = await conn.InsertAsync(conv);
            if (rowCount <= 0)
                throw new Exception("Error inserting ConversionRuleDollarValueModel");

            return conv;
        }
    }
}
