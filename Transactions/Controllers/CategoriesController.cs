using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using Transactions.Commands;
using Transactions.Files;
using Transactions.Mappings;
using Transactions.Mappings.Entities;
using Transactions.Models.Category;
using Transactions.Models.Transaction.Enums;
using Transactions.Services;
using Transactions.Validation;

namespace Transactions.Controllers{
    [ApiController]
//    [Route("categories")]
    public class CategoriesController : ControllerBase{
        private readonly ILogger<CategoriesController> _logger;
        private readonly IMapper _mapper;
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ILogger<CategoriesController> logger, IMapper mapper, ICategoriesService categoriesService){
            _logger = logger;
            _mapper = mapper;
            _categoriesService = categoriesService;
        }

        [HttpPost("categories/import")]
        public async Task<IActionResult> ImportCategories([FromForm(Name = "csv-file")] IFormFile csvFile){
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

        [HttpGet("spending-analytics")]
        public IActionResult ViewSpendingByCategory([FromQuery] string catcode, [FromQuery(Name = "start-date")] DateTime? startDate, [FromQuery(Name = "end-date")] DateTime? endDate, [FromQuery] DirectionsEnum? direction){
            var spendings = _categoriesService.GetSpendingsByCategory(catcode, startDate, endDate, direction);
            
            return Ok(JsonConvert.SerializeObject(spendings,Formatting.Indented));
        }
    }
}