using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Transactions.Models.Category{
    public class Category{
        [Required]
        [ReadOnly(true)]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string ParentCode { get; set; }
    }
}