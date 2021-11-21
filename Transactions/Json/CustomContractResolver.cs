using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Transactions.Json{
    public class CustomContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, 
                    Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            var list = base.CreateProperties(type, memberSerialization);
            for (int i = 0; i < list.Count; i++)
            {
                if(list[i].PropertyName == "amount"){
                    list[i].PropertyName = list[i].GetType().ToString();
                }
            } 

            return list;
        }
    }
}