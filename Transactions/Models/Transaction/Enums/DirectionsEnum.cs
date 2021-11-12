using System.ComponentModel;

namespace Transactions.Models.Transaction.Enums{
    public enum DirectionsEnum{
        [Description("Debit")]
        D,
        [Description("Credit")]
        C
    }
}