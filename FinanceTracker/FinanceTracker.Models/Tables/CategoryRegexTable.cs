using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FinanceTracker.Models
{
    internal class CategoryRegexTable
    {
        public async Task<List<CategoryRegexModel>> SelectConversionRuleRegexes(SQLiteAsyncConnection conn, Guid conversionRuleId)
        {
            return await conn.Table<CategoryRegexModel>().Where(o => o.CategoryConversionId == conversionRuleId).ToListAsync();
        }
    }
}
