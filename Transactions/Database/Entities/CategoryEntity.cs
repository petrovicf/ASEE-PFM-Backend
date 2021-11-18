using System.Collections.Generic;

namespace Transactions.Database.Entities{
    public class CategoryEntity{
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
        public List<TransactionEntity> Transaction { get; set; }
        public List<SplitEntity> Splits { get; set; }
    }
}