using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transactions.Database.Entities;
using Transactions.Models.Category;
using Transactions.Models.Transaction.Enums;

namespace Transactions.Database.Repositories{
    public interface ICategoriesRepository{
        Task<int> Insert(List<CategoryEntity> categoriesToInsert);
        SpendingsByCategory GetSpendings(string catcode, DateTime? startDate, DateTime? endDate, DirectionsEnum? direction);
        CategoryList<CategoryEntity> Get(string parentId);

    }
}