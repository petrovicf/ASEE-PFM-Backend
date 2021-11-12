using System.ComponentModel;

namespace Transactions.Problems{
    public enum ErrEnum{
        [Description("Value supplied does not meet minimum length")]
        MinLength,
        [Description("Value supplied exceeds maximum allowed length")]
        MaxLength,
        [Description("Mandatory field or parameter was not supplied")]
        Required,
        [Description("Value supplied was out of allowed range")]
        OutOfRange,
        [Description("Value supplied does not have expected format")]
        InvalidFormat,
        [Description("Value supplied does not belong to enumeration")]
        UnknownEnum,
        [Description("Value supplied does not belong to classification")]
        NotOnList,
        [Description("Value supplied does not conform to check digit validation")]
        CheckDigitInvalid,
        [Description("Parameter must be used with other parameters that were not supplied")]
        CombinationRequired,
        [Description("Parameter is read-only")]
        ReadOnly
    }
}