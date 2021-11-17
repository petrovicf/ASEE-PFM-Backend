using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions.Database.Entities;
using Transactions.Models.Category;
using Transactions.Models.Transaction.Enums;

namespace Transactions.Database.Repositories{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly TransactionsDbContext _dbContext;

        public CategoriesRepository(TransactionsDbContext dbContext){
            _dbContext = dbContext;
        }

        public SpendingsByCategory GetSpendings(string catcode, DateTime? startDate, DateTime? endDate, DirectionsEnum? direction)
        {
            startDate ??= new DateTime(2010, 1, 1);
            endDate ??= DateTime.Today.AddYears(5);

            var transactionsQuery = _dbContext.Transactions.AsQueryable().Where(t=>t.Date.Date > startDate.Value.Date && t.Date.Date < endDate.Value.Date);
            transactionsQuery = direction == null ? string.IsNullOrEmpty(catcode) ? transactionsQuery : transactionsQuery.Where(t=>t.Catcode==catcode)
                : string.IsNullOrEmpty(catcode) ? transactionsQuery.Where(t=>t.Direction==direction) : transactionsQuery.Where(t=>t.Direction==direction && t.Catcode==catcode);

            if(!string.IsNullOrEmpty(catcode)){
                return new SpendingsByCategory{
                    Groups = new List<SpendingInCategory>{
                        new SpendingInCategory{
                            Catcode = catcode,
                            Amount = Math.Round(transactionsQuery.Sum(t=>t.Amount), 2),
                            Count = transactionsQuery.Count()
                        }
                    }
                };
            }

            List<SpendingInCategory> spendings = new List<SpendingInCategory>();

            foreach (var group in transactionsQuery.AsEnumerable().GroupBy(t=>t.Catcode))
            {
                spendings.Add(new SpendingInCategory{
                    Catcode = group.Key,
                    Amount = Math.Round(group.Sum(t=>t.Amount), 2),
                    Count = group.Count()
                });
            }

            return new SpendingsByCategory{
                Groups = spendings
            };
        }

        public async Task<int> Insert(List<CategoryEntity> categoriesToInsert)
        {
            var categories = categoriesToInsert.GroupBy(c=>c.Code).Select(g=>g.Last()).ToList();

            _dbContext.Categories.UpdateRange(categories.Where(c=>_dbContext.Categories.Contains(c)));
            await _dbContext.Categories.AddRangeAsync(categories.Where(c=>!_dbContext.Categories.Contains(c)));
            await _dbContext.SaveChangesAsync();

            return 0;
        }
    }
}