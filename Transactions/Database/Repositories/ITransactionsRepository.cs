using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transactions.Commands;
using Transactions.Database.Entities;
using Transactions.Models.Categorization;
using Transactions.Models.Transaction;
using Transactions.Models.Transaction.Enums;
using Transactions.Problems;

namespace Transactions.Database.Repositories{
    public interface ITransactionsRepository{
        Task<int> Insert(List<TransactionEntity> transToInsert);
        Task<TransactionPagedList<TransactionEntity>> Get(List<TransactionKindsEnum> transactionKinds = null, DateTime? startDate=null, DateTime? endDate = null, int page = 1,
        int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc);
        Task<Problem> Categorize(string id, TransactionCategorizeCommand transactionCategorizeCommand);
        Task<Problem> Split(string id, SplitTransactionCommand splitTransactionCommand);
        Task<int> AutoCategorize(RulesList rulesList);
    }
}