using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Transactions.Models.Transaction.Enums{
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum SortOrder{
        [EnumMember(Value = "asc")]
        Asc,
        [EnumMember(Value = "desc")]
        Desc
    }
}