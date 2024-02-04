using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FinanceTracker.Models
{
    internal class ConversionRuleNameTable
    {
        public async Task<ConversionRuleNameModel> Select(SQLiteAsyncConnection conn, Guid accId)
        {
            ConversionRuleNameModel conv = await conn.Table<ConversionRuleNameModel>().Where((o) => o.AccountId == accId).FirstOrDefaultAsync();
            if(conv == null)
                return await InsertDefault(conn, accId);

            return conv;
        }

        public async Task<ConversionRuleNameModel> InsertDefault(SQLiteAsyncConnection conn, Guid accId)
        {
            ConversionRuleNameModel conv = new ConversionRuleNameModel 
            { 
                Id = Guid.NewGuid(), 
                AccountId = accId, 
                Column = 0,
                UseAdvanced = false, 
                AdvancedScript = "function GetName(splitTrans){\n\n}" 
            };

            int rowCount = await conn.InsertAsync(conv);
            if (rowCount <= 0)
                throw new Exception("Error inserting ConversionRuleNameModel");

            return conv;
        }
    }
}
