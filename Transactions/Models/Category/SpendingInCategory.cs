using Newtonsoft.Json;

namespace Transactions.Models.Category{
    public class SpendingInCategory{
        [JsonProperty("catcode")]
        public string Catcode { get; set; }
        [JsonProperty("amount")]
        public double Amount { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}