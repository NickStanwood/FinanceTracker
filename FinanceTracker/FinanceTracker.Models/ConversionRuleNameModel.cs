using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FinanceTracker.Models
{
    public class ConversionRuleNameModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(AccountModel))]
        public Guid AccountId { get; set; }
        public int Column { get; set; }

        public bool UseAdvanced { get; set; }
        public string AdvancedScript { get; set; }

        public string Convert(List<string> splitTrans)
        {
            if (UseAdvanced)
                throw new NotImplementedException();

            if (Column > splitTrans.Count - 1)
                throw new ArgumentOutOfRangeException();

            return splitTrans[Column];
        }
    }
}
