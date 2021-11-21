using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Transactions.Json{
    public class StringValueProvider : IValueProvider
    {
        private PropertyInfo _targetProperty;
        private string _substitutionValue;

        public StringValueProvider(PropertyInfo targetProperty, string substitutionValue)
        {
            _targetProperty = targetProperty;
            _substitutionValue = substitutionValue;
        }

        // SetValue gets called by Json.Net during deserialization.
        // The value parameter has the original value read from the JSON;
        // target is the object on which to set the value.
        public void SetValue(object target, object value)
        {
            _targetProperty.SetValue(target, value);
        }

        // GetValue is called by Json.Net during serialization.
        // The target parameter has the object from which to read the value;
        // the return value is what gets written to the JSON
        public object GetValue(object target)
        {
            object value = _targetProperty.GetValue(target);
            return value == null ? _substitutionValue : value;
        }
    }
}