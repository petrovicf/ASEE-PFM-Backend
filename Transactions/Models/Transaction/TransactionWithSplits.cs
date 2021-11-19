using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Transactions.Models.Category;
using Transactions.Models.Transaction.Enums;

namespace Transactions.Models.Transaction{
    public class TransactionWithSplits{
        [Required]
        [ReadOnly(true)]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("beneficiary-name")]
        public string BeneficiaryName { get; set; }

        [JsonProperty("date")]
        [Required]
        [DisplayFormat(DataFormatString ="{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [JsonProperty("direction")]
        [Required]
        public DirectionsEnum Direction { get; set; }

        [JsonProperty("amount")]
        [Required]
        public double Amount { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }

        [MinLength(3)]
        [MaxLength(3)]
        [Required]
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("mcc")]
        public MccCodeEnum? Mcc { get; set; }

        [Required]
        [JsonProperty("kind")]
        public TransactionKindsEnum Kind { get; set; }

        [JsonProperty("catcode")]
        [ReadOnly(true)]
        public string CatCode { get; set; }

        [JsonProperty("splits")]
        public List<SingleCategorySplit> Splits { get; set; }
    }
}