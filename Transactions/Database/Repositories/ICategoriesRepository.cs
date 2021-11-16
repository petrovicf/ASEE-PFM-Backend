using System.Collections.Generic;
using System.Threading.Tasks;
using Transactions.Database.Entities;

namespace Transactions.Database.Repositories{
    public interface ICategoriesRepository{
        Task<int> Insert(List<CategoryEntity> categoriesToInsert);
    }
}