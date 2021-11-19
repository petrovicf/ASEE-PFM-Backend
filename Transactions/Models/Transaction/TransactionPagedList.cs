using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Transactions.Models.Transaction.Enums;

namespace Transactions.Models.Transaction{
    public class TransactionPagedList<T>{
        private int totalCount;
        private int pageSize;
        private int page;
        private int totalPages;
        [JsonProperty("total-count")]
        public int TotalCount { get{return totalCount;} set{totalCount=value<0 ? 0 : value;}}
        [JsonProperty("page-size")]
        public int PageSize { get{return pageSize;} set{
            if(value<1){
                pageSize=1;
            }
            else if(value>100){
                pageSize=100;
            }
            else{
                pageSize=value;
            }
        } }
        [JsonProperty("page")]
        public int Page { get{return page;} set{page = value<1 ? 1 : value;} }
        [JsonProperty("total-pages")]
        public int TotalPages { get{return totalPages;} set{totalPages = value<0 ? 0 : value;} }
        [JsonProperty("sort-order")]
        public SortOrder SortOrder { get; set; }
        [JsonProperty("sort-by")]
        public string SortBy { get; set; }
        [JsonProperty("items")]
        public List<T> Items { get; set; }
    }
}