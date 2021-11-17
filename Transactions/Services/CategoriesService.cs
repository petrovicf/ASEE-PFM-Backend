using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Transactions.Database.Entities;
using Transactions.Database.Repositories;
using Transactions.Models.Category;
using Transactions.Models.Transaction.Enums;

namespace Transactions.Services{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IMapper _mapper;

        public CategoriesService(ICategoriesRepository categoriesRepository, IMapper mapper){
            _categoriesRepository = categoriesRepository;
            _mapper = mapper;
        }

        public SpendingsByCategory GetSpendingsByCategory(string catcode, DateTime? startDate, DateTime? endDate, DirectionsEnum? direction)
        {
            return _categoriesRepository.GetSpendings(catcode, startDate, endDate, direction);
        }

        public async Task<int> InsertCategories(List<Category> categories)
        {
            var categoriesToInsert = _mapper.Map<List<Category>, List<CategoryEntity>>(categories);

            await _categoriesRepository.Insert(categoriesToInsert);

            return 0;
        }
    }
}