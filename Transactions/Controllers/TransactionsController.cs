using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using Transactions.Commands;
using Newtonsoft.Json;
using Transactions.Database.Entities;
using Transactions.Files;
using Transactions.Mappings;
using Transactions.Mappings.Entities;
using Transactions.Models.Transaction.Enums;
using Transactions.Problems;
using Transactions.Services;
using Transactions.Validation;
using Newtonsoft.Json.Serialization;
using Transactions.Json;
using Transactions.Models.Categorization;
using System.Text.Json;

namespace Transactions.Controllers{
    [ApiController]
    //[Route("transactions")]
    public class TransactionsController:ControllerBase{
        private readonly ILogger<TransactionsController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionsService _transactionsService;

        public TransactionsController(ILogger<TransactionsController> logger, IMapper mapper, ITransactionsService transactionsService){
            _logger=logger;
            _mapper=mapper;
            _transactionsService=transactionsService;
        }

        [HttpPost("transactions/import")]
        public async Task<IActionResult> ImportTransactions([FromForm(Name = "csv-file")] IFormFile csvFile){
            if(csvFile==null){
                return BadRequest(JsonConvert.SerializeObject(new ValidationProblem{
                    Errors = new List<Errors>{
                        new Errors{
                            Tag = "csv-file",
                            Error = ErrEnum.Required,
                            Message = Validate.GetEnumDescription(ErrEnum.Required)
                        }
                    }
                },Formatting.Indented));
            }
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            CsvTransactionMapping csvTransactionMapping = new CsvTransactionMapping();
            CsvParser<TransactionCsvEntity> csvParser = new CsvParser<TransactionCsvEntity>(csvParserOptions,csvTransactionMapping);

            string filePath = await FileMethods.GetFilePath(csvFile);
            
            var transactionList = csvParser.ReadFromFile(filePath, Encoding.ASCII).ToList();

            var validationProblem = Validate.ValidateList<CsvMappingResult<TransactionCsvEntity>>(transactionList);

            if(validationProblem.Errors.Count>0){
                return BadRequest(JsonConvert.SerializeObject(validationProblem, Formatting.Indented));
            }

            var transactions = _mapper.Map<List<CsvMappingResult<TransactionCsvEntity>>, List<Models.Transaction.Transaction>>(transactionList);

            await _transactionsService.InsertTransactions(transactions);

            System.IO.File.Delete(filePath);

            return Ok("Transaction splitted");
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery(Name = "transaction-kinds")]List<TransactionKindsEnum> transactionKinds, [FromQuery(Name = "start-date")]DateTime? startDate, [FromQuery(Name = "end-date")]DateTime? endDate, [FromQuery]int? page,
        [FromQuery(Name = "page-size")]int? pageSize, [FromQuery(Name = "sort-by")]string sortBy, [FromQuery(Name = "sort-order")]SortOrder sortOrder){
            page ??= 1;
            pageSize ??= 10;

            var listToReturn = await _transactionsService.GetTransactions(transactionKinds, startDate, endDate, page.Value, pageSize.Value, sortBy, sortOrder);

            return Ok(JsonConvert.SerializeObject(listToReturn,Formatting.Indented, new JsonSerializerSettings{DateFormatString = "MM/dd/yyyy"}));
        }

        [HttpPost("transaction/{id}/categorize")]
        public async Task<IActionResult> CategorizeTransaction([FromRoute] string id, [FromBody] TransactionCategorizeCommand transactionCategorizeCommand){
            List<Errors> errors = new List<Errors>();
            if(string.IsNullOrEmpty(id)){
                errors.Add(new Errors{Tag = "id", Error = ErrEnum.Required, Message = Validate.GetEnumDescription(ErrEnum.Required)});
            }
            if(transactionCategorizeCommand == null){
                errors.Add(new Errors{Tag = "transaction-categorize-command", Error = ErrEnum.Required, Message = Validate.GetEnumDescription(ErrEnum.Required)});
            }
            if(errors.Count>0){
                return BadRequest(JsonConvert.SerializeObject(errors, Formatting.Indented));
            }

            var problem = await _transactionsService.CategorizeTransaction(id, transactionCategorizeCommand);

            if(problem!=null){
                if(problem is BusinessProblem){
                    Response.StatusCode = 440;
                    return new JsonResult(JsonConvert.SerializeObject(problem, Formatting.Indented));
                }
            }

            return Ok("Transaction splitted");
        }

        [HttpPost("transaction/{id}/split")]
        public async Task<IActionResult> SplitTransaction([FromRoute] string id, [FromBody] SplitTransactionCommand splitTransactionCommand){
            var problem = await _transactionsService.SplitTransaction(id, splitTransactionCommand);

            if(problem != null){
                if(problem is BusinessProblem){
                    Response.StatusCode = 440;
                    return new JsonResult(JsonConvert.SerializeObject(problem, Formatting.Indented));
                }
            }

            return Ok("Transaction splitted");
        }

        [HttpPost("transaction/auto-categorize")]
        public async Task<IActionResult> AutoCategorizeTransactions(){
            await _transactionsService.AutoCategorizeTransactions();

            return Ok();
        }
    }
}