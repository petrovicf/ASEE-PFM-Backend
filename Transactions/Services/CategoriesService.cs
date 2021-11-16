using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Transactions.Database.Entities;
using Transactions.Database.Repositories;
using Transactions.Models.Category;

namespace Transactions.Services{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IMapper _mapper;

        public CategoriesService(ICategoriesRepository categoriesRepository, IMapper mapper){
            _categoriesRepository = categoriesRepository;
            _mapper = mapper;
        }
        public async Task<int> InsertCategories(List<Category> categories)
        {
            var categoriesToInsert = _mapper.Map<List<Category>, List<CategoryEntity>>(categories);

            await _categoriesRepository.Insert(categoriesToInsert);

            return 0;
        }
    }
}