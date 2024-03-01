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
        public async Task<CategoryRegexModel?> Insert(SQLiteAsyncConnection conn, Guid conversionRuleId, Guid CategoryId, string regex)
        {
            CategoryRegexModel crm = new CategoryRegexModel
            {
                CategoryConversionId = conversionRuleId,
                CategoryId = CategoryId,
                Regex = regex
            };

            int rowCount = await conn.InsertAsync(crm);
            if (rowCount > 0)
                return crm;

            return null;
        }
    }
}
