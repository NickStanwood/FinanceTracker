using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;


namespace FinanceTracker.Models
{
    internal class ConversionRuleBalanceTable
    {
        public async Task<ConversionRuleBalanceModel> Select(SQLiteAsyncConnection conn, Guid accId)
        {
            ConversionRuleBalanceModel conv = await conn.Table<ConversionRuleBalanceModel>().Where((o) => o.AccountId == accId).FirstOrDefaultAsync();
            if (conv == null)
                return await InsertDefault(conn, accId);

            return conv;
        }

        public async Task<ConversionRuleBalanceModel> InsertDefault(SQLiteAsyncConnection conn, Guid accId)
        {
            ConversionRuleBalanceModel conv = new ConversionRuleBalanceModel
            {
                Id = Guid.NewGuid(),
                AccountId = accId,
                Column = 0,
                ApplyNegation = false,
                AdditiveUpdate = false,
                UseAdvanced = false,
                AdvancedScript = "function GetNewBalance(splitTrans, currBalance){\n\n}"
            };

            int rowCount = await conn.InsertAsync(conv);
            if (rowCount <= 0)
                throw new Exception("Error inserting ConversionRuleNameModel");

            return conv;
        }
    }
}
