using System.Collections.Generic;
using System.Threading.Tasks;
using Transactions.Database.Entities;

namespace Transactions.Database.Repositories{
    public class TransactionsRepository : ITransactionsRepository{
        private readonly TransactionsDbContext _dbContext;

        public TransactionsRepository(TransactionsDbContext dbContext){
            _dbContext=dbContext;
        }

        public async Task<int> Insert(List<TransactionEntity> transToInsert)
        {
            await _dbContext.Transactions.AddRangeAsync(transToInsert);

            await _dbContext.SaveChangesAsync();
            return 0;
        }
    }
}