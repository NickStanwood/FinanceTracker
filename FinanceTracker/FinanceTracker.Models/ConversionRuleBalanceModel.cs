using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FinanceTracker.Models
{
    public class ConversionRuleBalanceModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(AccountModel))]
        public Guid AccountId { get; set; }
        public int Column { get; set; }
        public bool ApplyNegation { get; set; }
        public bool DataAvailable { get; set; }

        public bool UseAdvanced { get; set; }
        public string AdvancedScript { get; set; }
        public double? Convert(List<string> splitTrans)
        {
            if (!DataAvailable)
                return null;

            if (UseAdvanced)
                throw new NotImplementedException();

            if (Column > splitTrans.Count - 1)
                throw new ArgumentOutOfRangeException();

            double val;
            if (!double.TryParse(splitTrans[Column], out val))
                throw new FormatException();
            
            if (ApplyNegation)
                val *= -1;

            return val;
        }
    }
}
