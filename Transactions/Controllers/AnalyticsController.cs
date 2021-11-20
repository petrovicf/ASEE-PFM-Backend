using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Transactions.Models.Transaction.Enums;
using Transactions.Services;

namespace Transactions.Controllers{
    [ApiController]
    [Route("spending-analytics")]
    public class AnalyticsController : ControllerBase{
        private readonly ILogger<AnalyticsController> _logger;
        private readonly ICategoriesService _categoriesService;

        public AnalyticsController(ILogger<AnalyticsController> logger, ICategoriesService categoriesService){
            _logger = logger;
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public IActionResult ViewSpendingByCategory([FromQuery] string catcode, [FromQuery(Name = "start-date")] DateTime? startDate, [FromQuery(Name = "end-date")] DateTime? endDate, [FromQuery] DirectionsEnum? direction){
            var spendings = _categoriesService.GetSpendingsByCategory(catcode, startDate, endDate, direction);
            
            return Ok(JsonConvert.SerializeObject(spendings,Formatting.Indented));
        }
    }
}