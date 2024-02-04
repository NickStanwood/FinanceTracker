using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FinanceTracker.Models
{
    internal class ConversionRuleDateTable
    {
        public async Task<ConversionRuleDateModel> Select(SQLiteAsyncConnection conn, Guid accId)
        {
            ConversionRuleDateModel conv = await conn.Table<ConversionRuleDateModel>().Where((o) => o.AccountId == accId).FirstOrDefaultAsync();
            if (conv == null)
                return await InsertDefault(conn, accId);

            return conv;
        }
        public async Task<ConversionRuleDateModel> InsertDefault(SQLiteAsyncConnection conn, Guid accId)
        {
            ConversionRuleDateModel conv = new ConversionRuleDateModel 
            { 
                Id = Guid.NewGuid(), 
                AccountId = accId, 
                Column = 0,
                DateFormat="", 
                UseAdvanced = false, 
                AdvancedScript = "function GetDate(splitTrans){\n\n}" 
            };

            int rowCount = await conn.InsertAsync(conv);
            if (rowCount <= 0)
                throw new Exception("Error inserting ConversionRuleDateModel");
            
            return conv;
        }
    }
}

