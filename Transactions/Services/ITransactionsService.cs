using System.Collections.Generic;
using System.Threading.Tasks;
using Transactions.Models.Transaction;

namespace Transactions.Services{
    public interface ITransactionsService{
        Task<int> InsertTransactions(List<Transaction> transactions);
    }
}