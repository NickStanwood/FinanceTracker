using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Globalization;

namespace FinanceTracker.Models
{
    public class ConversionRuleDateModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(AccountModel))]
        public Guid AccountId { get; set; }
        public int Column { get; set; }
        public string DateFormat { get; set; }

        public bool UseAdvanced { get; set; }
        public string AdvancedScript { get; set; }
        
        public DateTime Convert(List<string> splitTrans)
        {
            if (UseAdvanced)
                throw new NotImplementedException();

            if (Column > splitTrans.Count - 1)
                throw new ArgumentOutOfRangeException();

            DateTime time;
            if(DateFormat == null || DateFormat == "")
            {
                if (DateTime.TryParse(splitTrans[Column], out time))
                    return time;
            }
            else
            {
                if (DateTime.TryParseExact(splitTrans[Column], DateFormat, null, DateTimeStyles.None, out time))
                    return time;
            }

            throw new FormatException();
        }

    }
}
