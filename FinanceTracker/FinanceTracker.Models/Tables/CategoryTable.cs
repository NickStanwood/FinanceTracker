using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FinanceTracker.Models
{
    internal class CategoryTable
    {
        public async Task<List<CategoryModel>> SelectAll(SQLiteAsyncConnection conn)
        {
            return await conn.Table<CategoryModel>().ToListAsync();
        }
        public async Task<CategoryModel> Select(SQLiteAsyncConnection conn, Guid categoryId)
        {
            return await conn.Table<CategoryModel>().Where(o => o.Id == categoryId).FirstAsync();
        }
    }
}
