using Newtonsoft.Json;

namespace Transactions.Problems{
    public class BusinessProblem : Problem{
        [JsonProperty("problem-literal")]
        public string ProblemLiteral { get; set; }
        [JsonProperty("problem-message")]
        public string ProblemMessage { get; set; }
        [JsonProperty("problem-details")]
        public string ProblemDetails { get; set; }
    }
}