using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Transactions.Database.Entities;
using Transactions.Models.Transaction;
using Transactions.Models.Transaction.Enums;

namespace Transactions.Database.Repositories{
    public class TransactionsRepository : ITransactionsRepository{
        private readonly TransactionsDbContext _dbContext;

        public TransactionsRepository(TransactionsDbContext dbContext){
            _dbContext=dbContext;
        }

        public async Task<TransactionPagedList<TransactionEntity>> Get(List<TransactionKindsEnum> transactionKinds = null, DateTime? startDate=null, DateTime? endDate = null, int page = 1,
        int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc)
        {
            startDate ??= new DateTime(2010, 1, 1);
            endDate ??= DateTime.Today;

            var query = _dbContext.Transactions.AsQueryable();

            if(transactionKinds!=null && transactionKinds.Count>0){
                query = query.Where(t=>transactionKinds.Contains(t.Kind) && t.Date.Date >= startDate.Value.Date);
            }

            var totalCount = await query.CountAsync();
            var totalPages = Math.Ceiling((double)totalCount/pageSize);

            if(!string.IsNullOrEmpty(sortBy)){
                if(sortOrder==SortOrder.Desc){
                    query = query.OrderByDescending(sortBy, t=>t.Id);
                }
                else{
                    query = query.OrderBy(sortBy, t=>t.Id);
                }
            }

            query = query.Skip((page-1) * pageSize).Take(pageSize);
            var pagesSortedTransactionsList = await query.ToListAsync();

            return new TransactionPagedList<TransactionEntity>{
                TotalCount = totalCount,
                PageSize = pageSize,
                Page = page,
                TotalPages = (int)totalPages,
                SortOrder = sortOrder,
                SortBy = sortBy,
                Items=pagesSortedTransactionsList
            };
        }

        public async Task<int> Insert(List<TransactionEntity> transToInsert)
        {
            await _dbContext.Transactions.AddRangeAsync(transToInsert);

            await _dbContext.SaveChangesAsync();
            return 0;
        }
    }
}