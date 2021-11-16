using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using Transactions.Commands;
using Transactions.Files;
using Transactions.Mappings;
using Transactions.Mappings.Entities;
using Transactions.Models.Category;
using Transactions.Services;
using Transactions.Validation;

namespace Transactions.Controllers{
    [ApiController]
    [Route("categories")]
    public class CategoriesController : ControllerBase{
        private readonly ILogger<CategoriesController> _logger;
        private readonly IMapper _mapper;
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ILogger<CategoriesController> logger, IMapper mapper, ICategoriesService categoriesService){
            _logger = logger;
            _mapper = mapper;
            _categoriesService = categoriesService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCategories([FromForm] IFormFile csvFile){
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            CsvCategoryMapping csvCategoryMapping = new CsvCategoryMapping();
            CsvParser<CategoryCsv> csvParser = new CsvParser<CategoryCsv>(csvParserOptions, csvCategoryMapping);

            string filePath = await FileMethods.GetFilePath(csvFile);

            var categoryList = csvParser.ReadFromFile(filePath, Encoding.ASCII).ToList();

            var validationProblem = Validate.ValidateList<CsvMappingResult<CategoryCsv>>(categoryList);

            if(validationProblem.Errors.Count>0){
                return BadRequest(validationProblem);
            }

            var categories = _mapper.Map<List<CsvMappingResult<CategoryCsv>>, List<Category>>(categoryList);

            await _categoriesService.InsertCategories(categories);

            System.IO.File.Delete(filePath);

            return Ok("Categories imported");
        }

        [HttpPost("{id}/categorize")]
        public IActionResult CategorizeTransaction([FromRoute] string id, [FromBody] TransactionCategorizeCommand transactionCategorizeCommand){
            return Ok();
        }
    }
}