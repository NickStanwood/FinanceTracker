using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FinanceTracker.Models
{
    public class CategoryModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }

        [ForeignKey(typeof(CategoryModel))]
        public Guid ParentCategoryId { get; set; }
    }
}
