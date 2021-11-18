using System.Collections.Generic;
using Transactions.Models.Category;

namespace Transactions.Commands{
    public class SplitTransactionCommand{
        public List<SingleCategorySplit> Splits { get; set; }
    }
}