using System.Text.RegularExpressions;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FinanceTracker.Models
{
    public class ConversionRuleCategoryModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(AccountModel))]
        public Guid AccountId { get; set; }
        public int Column { get; set; }

        public bool UseAdvanced { get; set; }
        public string AdvancedScript { get; set; }

        public async Task<CategoryModel?> Convert(List<string> splitTrans, List<CategoryRegexModel> regexes)
        {
            if (UseAdvanced)
                throw new NotImplementedException();

            if (Column > splitTrans.Count - 1)
                throw new ArgumentOutOfRangeException();

            foreach(CategoryRegexModel rm in regexes)
            {
                Regex reg = new Regex(rm.Regex);
                if(reg.IsMatch(splitTrans[Column]))
                {
                    CategoryModel model = await SQLiteContext.GetCategory(rm.CategoryId);
                }
            }

            return null;
        }
    }
}
