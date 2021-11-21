using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Transactions.Models.Transaction.Enums{
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))] 
    public enum DirectionsEnum{
        [Description("Debit")]
        [EnumMember(Value = "d")]
        D,
        [Description("Credit")]
        [EnumMember(Value = "c")]
        C
    }
}