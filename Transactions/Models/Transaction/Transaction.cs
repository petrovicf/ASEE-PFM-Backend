using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Transactions.Models.Transaction.Enums;

namespace Transactions.Models.Transaction{
    public class Transaction{
        [Required]
        [ReadOnly(true)]
        public string Id { get; set; }

        public string BeneficiaryName { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public DirectionsEnum Direction { get; set; }

        [Required]
        public double Amount { get; set; }
        
        public string Description { get; set; }

        [MinLength(3)]
        [MaxLength(3)]
        [Required]
        public string Currency { get; set; }

        public MccCodeEnum? Mcc { get; set; }

        [Required]
        public TransactionKindsEnum Kind { get; set; }

        /*[ReadOnly(true)]
        public string CatCode { get; set; }*/
    }
}