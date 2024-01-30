using FinanceTracker.Models;

namespace FinanceTracker.Core
{
    public class CategoryStats
    {
        public CategoryModel Category { get; }
        public double AverageTransaction { get; }
        public int TransactionCount { get; }
        public double TotalTransactionValues { get; }
        public CategoryStats(CategoryModel category, IList<TransactionModel> transactions)
        {
            Category = category;
            TransactionCount = 0;
            foreach(TransactionModel trans in transactions)
            {
                if(trans.CategoryId == Category.Id)
                {
                    TransactionCount++;
                    TotalTransactionValues += trans.DollarValue;
                }
            }

            if(TransactionCount > 0)
                AverageTransaction = TotalTransactionValues / TransactionCount;
        }
    }
}