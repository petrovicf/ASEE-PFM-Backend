using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Transactions.Models.Category{
    public class SingleCategorySplit{
        [Required]
        [JsonProperty("catcode")]
        public string Catcode { get; set; }
        [Required]
        [JsonProperty("amount")]
        public double Amount { get; set; }
    }
}