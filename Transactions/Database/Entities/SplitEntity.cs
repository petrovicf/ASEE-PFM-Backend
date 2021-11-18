namespace Transactions.Database.Entities{
    public class SplitEntity{
        public string TransactionId { get; set; }
        public string Catcode { get; set; }
        public double Amount { get; set; }
        public TransactionEntity Transaction { get; set; }
        public CategoryEntity Category { get; set; }
    }
}