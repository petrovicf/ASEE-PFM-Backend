using System.ComponentModel.DataAnnotations;

namespace Transactions.Problems{
    public class Errors{
        [Required]
        public string Tag { get; set; }
        [Required]
        public ErrEnum Error { get; set; }
        [Required]
        public string Message { get; set; }
    }
}