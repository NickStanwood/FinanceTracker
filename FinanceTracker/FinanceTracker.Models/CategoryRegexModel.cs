using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FinanceTracker.Models
{
    public class CategoryRegexModel
    {
        [ForeignKey(typeof(ConversionRuleCategoryModel))]
        public Guid CategoryConversionId { get; set; }

        [ForeignKey(typeof(CategoryModel))]
        public Guid CategoryId { get; set; }

        public string Regex { get; set; }
    }
}
