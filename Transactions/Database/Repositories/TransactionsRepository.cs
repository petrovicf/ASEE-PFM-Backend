using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Transactions.Commands;
using Transactions.Database.Entities;
using Transactions.Models.Transaction;
using Transactions.Models.Transaction.Enums;
using Transactions.Problems;

namespace Transactions.Database.Repositories{
    public class TransactionsRepository : ITransactionsRepository{
        private readonly TransactionsDbContext _dbContext;

        public TransactionsRepository(TransactionsDbContext dbContext){
            _dbContext=dbContext;
        }

        public async Task<Problem> Categorize(string id, TransactionCategorizeCommand transactionCategorizeCommand)
        {
            if(!await _dbContext.Categories.AnyAsync(c=>c.Code==transactionCategorizeCommand.Catcode)){
                return new BusinessProblem{
                    ProblemLiteral = "provided-category-does-not-exist",
                    ProblemMessage = "Provided category does not exist",
                    ProblemDetails = $"Category with code {transactionCategorizeCommand.Catcode} does not exist in database"
                };
            }
            if(!await _dbContext.Transactions.AnyAsync(t=>t.Id==id)){
                return new BusinessProblem{
                    ProblemLiteral = "provided-transaction-does-not-exist",
                    ProblemMessage = "Provided transaction does not exist",
                    ProblemDetails = $"Transaction with id {id} does not exist in database"
                };
            }

            var categorizedTransaction = await _dbContext.Transactions.FirstAsync(t=>t.Id==id);
            categorizedTransaction.Catcode = transactionCategorizeCommand.Catcode;

            _dbContext.Update(categorizedTransaction);
            await _dbContext.SaveChangesAsync();

            return null;
        }

        public async Task<TransactionPagedList<TransactionEntity>> Get(List<TransactionKindsEnum> transactionKinds = null, DateTime? startDate=null, DateTime? endDate = null, int page = 1,
        int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc)
        {
            startDate ??= new DateTime(2010, 1, 1);
            endDate ??= DateTime.Today.AddYears(5);

            var query = _dbContext.Transactions.AsQueryable();

            if(transactionKinds!=null && transactionKinds.Count>0){
                query = query.Where(t=>transactionKinds.Contains(t.Kind) && t.Date.Date >= startDate.Value.Date && t.Date.Date <= endDate.Value.Date);
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

        public async Task<Problem> Split(string id, SplitTransactionCommand splitTransactionCommand)
        {
            var notExisting = splitTransactionCommand.Splits.Where(s=>!_dbContext.Categories.Any(c=>c.Code==s.Catcode));
            if(notExisting.Count()>0){
                return new BusinessProblem{
                    ProblemLiteral = "provided-category-does-not-exist",
                    ProblemMessage = "Provided category does not exist",
                    ProblemDetails = $"Category with code {notExisting.First().Catcode} does not exist in database"
                };
            }
            if(!await _dbContext.Transactions.AnyAsync(t=>t.Id==id)){
                return new BusinessProblem{
                    ProblemLiteral = "provided-transaction-does-not-exist",
                    ProblemMessage = "Provided transaction does not exist",
                    ProblemDetails = $"Transaction with id {id} does not exist in database"
                };
            }

            var splitsQuery = splitTransactionCommand.Splits.AsQueryable();
            if(splitsQuery.Sum(s=>s.Amount) > _dbContext.Transactions.Where(t=>t.Id==id).Sum(t=>t.Amount)){
                return new BusinessProblem{
                    ProblemLiteral = "split-amount-over-transaction-amount",
                    ProblemMessage = "Split amount is larger then transaction amount",
                    ProblemDetails = ""
                };
            }

            return null;
        }
    }
}