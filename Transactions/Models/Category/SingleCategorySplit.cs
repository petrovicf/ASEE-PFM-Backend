using System.ComponentModel.DataAnnotations;

namespace Transactions.Models.Category{
    public class SingleCategorySplit{
        [Required]
        public string Catcode { get; set; }
        [Required]
        public double Amount { get; set; }
    }
}