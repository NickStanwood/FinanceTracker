using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FinanceTracker.Models
{
    internal class ConversionRuleSplitterTable
    {
        public async Task<ConversionRuleSplitterModel> Select(SQLiteAsyncConnection conn, Guid accId)
        {
            ConversionRuleSplitterModel conv = await conn.Table<ConversionRuleSplitterModel>().Where((o) => o.AccountId == accId).FirstOrDefaultAsync();
            if (conv == null)
                return await InsertDefault(conn, accId);

            return conv;
        }

        public async Task<ConversionRuleSplitterModel> InsertDefault(SQLiteAsyncConnection conn, Guid accId)
        {
            ConversionRuleSplitterModel conv = new ConversionRuleSplitterModel
            {
                Id = Guid.NewGuid(),
                AccountId = accId,
                DelimChar = ",",
                IgnoreDelimInQuotes = false,
                UseAdvanced = false,
                AdvancedScript = "function SplitRawTransaction(rawTrans){\n\n}"
            };

            int rowCount = await conn.InsertAsync(conv);
            if (rowCount <= 0)
                throw new Exception("Error inserting ConversionRuleSplitterModel");

            return conv;
        }
    }
}
