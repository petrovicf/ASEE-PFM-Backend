using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transactions.Models.Category;
using Transactions.Models.Transaction.Enums;

namespace Transactions.Services{
    public interface ICategoriesService{
        Task<int> InsertCategories(List<Category> categories);
        SpendingsByCategory GetSpendingsByCategory(string catcode, DateTime? startDate, DateTime? endDate, DirectionsEnum? direction);
        CategoryList<Category> GetCategories(string parentId);
    }
}