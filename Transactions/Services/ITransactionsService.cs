using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transactions.Models.Transaction;
using Transactions.Models.Transaction.Enums;

namespace Transactions.Services{
    public interface ITransactionsService{
        Task<int> InsertTransactions(List<Transaction> transactions);
        Task<TransactionPagedList<Transaction>> GetTransactions(List<TransactionKindsEnum> transactionKinds = null, DateTime? startDate=null, DateTime? endDate = null, int page = 1,
        int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc);
    }
}