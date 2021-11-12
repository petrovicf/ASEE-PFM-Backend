using System.Collections.Generic;
using System.Threading.Tasks;
using Transactions.Database.Entities;

namespace Transactions.Database.Repositories{
    public interface ITransactionsRepository{
        Task<int> Insert(List<TransactionEntity> transToInsert);
    }
}