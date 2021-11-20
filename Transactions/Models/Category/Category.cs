using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Transactions.Models.Category{
    public class Category{
        [Required]
        [ReadOnly(true)]
        [JsonProperty("code")]
        public string Code { get; set; }
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("parent-code")]
        public string ParentCode { get; set; }
    }
}