using System.Collections.Generic;
using System.Threading.Tasks;
using Transactions.Models.Category;

namespace Transactions.Services{
    public interface ICategoriesService{
        Task<int> InsertCategories(List<Category> categories);
    }
}