using Transactions.Models.Transaction.Enums;

namespace Transactions.Mappings.Entities{
    public class TransactionCsvEntity{
        public string Id { get; set; }

        public string BeneficiaryName { get; set; }

        public string Date { get; set; }

        public DirectionsEnum Direction { get; set; }

        public string Amount { get; set; }
        
        public string Description { get; set; }

        public string Currency { get; set; }

        public string Mcc { get; set; }

        public TransactionKindsEnum Kind { get; set; }
    }
}