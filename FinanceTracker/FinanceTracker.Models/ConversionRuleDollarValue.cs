using SQLite;

namespace FinanceTracker.Models
{
    public class ConversionRuleDollarValue
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        //foreignkey AccountModel.Id
        public Guid AccountId { get; set; }
        public int Column { get; set; }
        public bool ApplyNegation { get; set; }

        public bool UseAdvanced { get; set; }
        public string AdvancedScript { get; set; }
    }
}
