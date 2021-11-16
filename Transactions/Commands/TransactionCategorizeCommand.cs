using System.ComponentModel.DataAnnotations;

namespace Transactions.Commands{
    public class TransactionCategorizeCommand{
        [Required]
        public string Catcode { get; set; }
    }
}