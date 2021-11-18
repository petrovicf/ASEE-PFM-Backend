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
        public int TotalCount { get{return totalCount;} set{totalCount=value<0 ? 0 : value;}}
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
        public int Page { get{return page;} set{page = value<1 ? 1 : value;} }
        public int TotalPages { get{return totalPages;} set{totalPages = value<0 ? 0 : value;} }
        public SortOrder SortOrder { get; set; }
        public string SortBy { get; set; }
        public List<T> Items { get; set; }
    }
}