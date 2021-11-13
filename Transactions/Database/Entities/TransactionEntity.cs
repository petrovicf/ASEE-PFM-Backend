using System;
using Transactions.Models.Transaction.Enums;

namespace Transactions.Database.Entities{
    public class TransactionEntity{
        public string Id { get; set; }

        public string BeneficiaryName { get; set; }

        public DateTime Date { get; set; }

        public DirectionsEnum Direction { get; set; }

        public double Amount { get; set; }
        
        public string Description { get; set; }

        public string Currency { get; set; }

        public MccCodeEnum? Mcc { get; set; }

        public TransactionKindsEnum Kind { get; set; }

        //public string CatCode { get; set; }
    }
}