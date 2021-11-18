using System.ComponentModel;
using Newtonsoft.Json;

namespace Transactions.Models.Transaction.Enums{
    public enum TransactionKindsEnum{
        [Description("Deposit")]
        Dep,

        [Description("Withdrawal")]
        wdw,

        [Description("Payment")]
        Pmt,

        [Description("Fee")]
        Fee,

        [Description("Interest credit")]
        Inc,

        [Description("Reversal")]
        Rev,

        [Description("Adjustment")]
        Adj,

        [Description("Loan disbursement")]
        Lnd,

        [Description("Loan repayment")]
        Lnr,

        [Description("Foreign currency exchange")]
        Fcx,

        [Description("Account opening")]
        Aop,

        [Description("Account closing")]
        Acl,

        [Description("Split payment")]
        Spl,

        [Description("Salary")]
        Sal
    }
}