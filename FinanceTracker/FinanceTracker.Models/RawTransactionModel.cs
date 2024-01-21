using SQLite;

namespace FinanceTracker.Models
{
    public class RawTransactionModel
    {
        [PrimaryKey, AutoIncrement]
        public int TransactionId { get; set; }
        public Guid AccountId {get;set;}
        public string RawTransaction { get; set; }
    }
}
