using System.Collections.Generic;
using Newtonsoft.Json;

namespace Transactions.Models.Category{
    public class SpendingsByCategory{
        [JsonProperty("groups")]
        public List<SpendingInCategory> Groups { get; set; }
    }
}