using System.Collections.Generic;
using Newtonsoft.Json;

namespace Transactions.Models.Category{
    public class CategoryList<T>{
        [JsonProperty("items")]
        public List<T> Items { get; set; }
    }
}