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
        public async Task<List<CategoryModel>> SelectAllChildren(SQLiteAsyncConnection conn, Guid? parentCategoryId)
        {
            return await conn.Table<CategoryModel>().Where(o => o.ParentCategoryId == parentCategoryId).ToListAsync();
        }

        public async Task<CategoryModel> Select(SQLiteAsyncConnection conn, Guid categoryId)
        {
            return await conn.Table<CategoryModel>().Where(o => o.Id == categoryId).FirstAsync();
        }
        public async Task<CategoryModel?> Insert(SQLiteAsyncConnection conn, Guid? parentId, string name)
        {
            var cm = new CategoryModel
            {
                Id = Guid.NewGuid(),
                Name = name,
                ParentCategoryId = parentId
            };

            int rowCount = await conn.InsertAsync(cm);
            if (rowCount > 0)
                return cm;

            return null;
        }
        public async Task<CategoryModel?> Update(SQLiteAsyncConnection conn, CategoryModel cm)
        {
            int rowCount = await conn.UpdateAsync(cm);
            if (rowCount > 0)
                return cm;

            return null;
        }
    }
}
