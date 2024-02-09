using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FinanceTracker.Models
{
    public class TransactionModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(AccountModel))]
        public Guid AccountId { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }

        [ForeignKey(typeof(CategoryModel))]
        public Guid? CategoryId { get; set; }
        public double DollarValue { get; set; }
        public double? Balance { get; set; }
    }
}