using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Transactions.Models.Transaction.Enums{
    [JsonConverter(typeof(StringEnumConverter))] 
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