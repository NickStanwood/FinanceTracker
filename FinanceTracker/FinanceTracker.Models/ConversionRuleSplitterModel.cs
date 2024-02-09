using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FinanceTracker.Models
{
    public class ConversionRuleSplitterModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(AccountModel))]
        public Guid AccountId { get; set; }
        public string DelimChar { get; set; }
        public bool IgnoreDelimInQuotes { get; set; }

        public bool UseAdvanced { get; set; }
        public string AdvancedScript { get; set; }

        public string[] Convert(string rawTrans)
        {
            if (UseAdvanced)
                throw new NotImplementedException();

            if(IgnoreDelimInQuotes)
                throw new NotImplementedException();

            return rawTrans.Split(DelimChar);
        }
    }
}
