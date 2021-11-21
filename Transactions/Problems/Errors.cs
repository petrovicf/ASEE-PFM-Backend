using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Transactions.Problems{
    public class Errors{
        [Required]
        [JsonProperty("tag")]
        public string Tag { get; set; }
        [Required]
        [JsonProperty("error")]
        public ErrEnum Error { get; set; }
        [Required]
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}