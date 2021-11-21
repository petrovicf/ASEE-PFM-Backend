using System.Collections.Generic;
using Newtonsoft.Json;

namespace Transactions.Problems{
    public class ValidationProblem : Problem{
        [JsonProperty("errors")]
        public List<Errors> Errors { get; set; }
    }
}