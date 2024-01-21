using SQLite;

namespace FinanceTracker.Models
{
    public class TransactionModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public Guid AccountId { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double DollarValue { get; set; }
        public List<string> Tags { get; set; }
    }
}