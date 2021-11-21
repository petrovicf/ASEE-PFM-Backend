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

        public CategoryList<CategoryEntity> Get(string parentId)
        {
            var categoriesQuery = _dbContext.Categories.AsQueryable();

            if(!string.IsNullOrEmpty(parentId)){
                categoriesQuery = categoriesQuery.Where(c=>c.ParentCode==parentId);
            }

            return new CategoryList<CategoryEntity>{
                Items = categoriesQuery.ToList()
            };
        }

        public SpendingsByCategory GetSpendings(string catcode, DateTime? startDate, DateTime? endDate, DirectionsEnum? direction)
        {
            startDate ??= new DateTime(2010, 1, 1);
            endDate ??= DateTime.Today.AddYears(5);

            var transactionsQuery = _dbContext.Transactions.AsQueryable().Where(t=>t.Date.Date > startDate.Value.Date && t.Date.Date < endDate.Value.Date && t.Catcode!=null);
            var categoriesQuery = _dbContext.Categories.AsQueryable();
            transactionsQuery = direction == null ? string.IsNullOrEmpty(catcode) ? transactionsQuery : transactionsQuery.Where(t=>categoriesQuery.Any(c=>c.ParentCode==catcode && c.Code==t.Catcode))
                : string.IsNullOrEmpty(catcode) ? transactionsQuery.Where(t=>t.Direction==direction) : transactionsQuery.Where(t=>t.Direction==direction && categoriesQuery.Any(c=>c.ParentCode==catcode && c.Code==t.Catcode));

            List<SpendingInCategory> spendings = new List<SpendingInCategory>();
            IEnumerable<TransactionEntity> query;
            string key;

            if(!string.IsNullOrEmpty(catcode)){
                foreach (var group in transactionsQuery.AsEnumerable().GroupBy(t=>t.Catcode))
                {
                    spendings.Add(new SpendingInCategory{
                        Catcode = group.Key,
                        Amount = Math.Round(group.Sum(t=>t.Amount), 2),
                        Count = group.Count()
                    });
                }
            }
            else{
                foreach (var group in transactionsQuery.AsEnumerable().GroupBy(t=>t.Catcode))
                {
                    key = group.Key;
                    query = group.AsEnumerable().Union(transactionsQuery.Where(t=>categoriesQuery.Any(c=>c.ParentCode==key && c.Code==t.Catcode)));
                    spendings.Add(new SpendingInCategory{
                        Catcode = key,
                        Amount = Math.Round(query.Sum(t=>t.Amount), 2),
                        Count = query.Count()
                    });
                }
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