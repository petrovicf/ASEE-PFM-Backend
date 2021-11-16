using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions.Database.Entities;

namespace Transactions.Database.Repositories{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly TransactionsDbContext _dbContext;

        public CategoriesRepository(TransactionsDbContext dbContext){
            _dbContext = dbContext;
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