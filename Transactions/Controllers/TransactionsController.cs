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
using Transactions.Database.Entities;
using Transactions.Mappings;
using Transactions.Mappings.Entities;
using Transactions.Models.Transaction.Enums;
using Transactions.Problems;
using Transactions.Services;

namespace Transactions.Controllers{
    [ApiController]
    [Route("v1/transactions")]
    public class TransactionsController:ControllerBase{
        private readonly ILogger<TransactionsController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionsService _transactionsService;

        public TransactionsController(ILogger<TransactionsController> logger, IMapper mapper, ITransactionsService transactionsService){
            _logger=logger;
            _mapper=mapper;
            _transactionsService=transactionsService;
        }

        private async Task<string> GetFilePath(IFormFile file){
            var filePath = Path.GetTempFileName();

            var stream = System.IO.File.Create(filePath);

            await file.CopyToAsync(stream);

            stream.Close();

            return filePath;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactions([FromForm] IFormFile csvFile){
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            CsvTransactionMapping csvTransactionMapping = new CsvTransactionMapping();
            CsvParser<TransactionCsvEntity> csvParser = new CsvParser<TransactionCsvEntity>(csvParserOptions,csvTransactionMapping);

            string filePath = await GetFilePath(csvFile);

            var transactionList = csvParser.ReadFromFile(filePath, Encoding.ASCII).ToList();

            var validationProblem = Validate(transactionList);

            if(validationProblem.Errors.Count>0){
                return BadRequest(validationProblem);
            }

            var transactions = _mapper.Map<List<CsvMappingResult<TransactionCsvEntity>>, List<Models.Transaction.Transaction>>(transactionList);

            transactions.RemoveAt(transactions.Count-1);
            //await _transactionsService.InsertTransactions(transactions);

            System.IO.File.Delete(filePath);

            return Ok("Transaction splitted");
        }

        private ValidationProblem Validate(List<CsvMappingResult<TransactionCsvEntity>> list){
            List<Errors> errors = new List<Errors>();

            foreach(var item in list){
                if(string.IsNullOrEmpty(item.Result.Id)){
                    errors.Add(CreateError("id", ErrEnum.Required, GetEnumDescription(ErrEnum.Required)));
                    continue;
                }
            }

            return new ValidationProblem{
                Errors = errors
            };
        }

        private Errors CreateError(string tag, ErrEnum err, string message){
            return new Errors{
                Tag = tag,
                Error = err,
                Message = message
            };
        }

        private string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery]List<TransactionKindsEnum> transactionKinds, [FromQuery]DateTime startDate, [FromQuery]DateTime endDate, [FromQuery]int? page,
        [FromQuery]int? pageSize, [FromQuery]string sortBy, [FromQuery]SortOrder sortOrder){
            page ??= 1;
            pageSize ??= 10;

            var listToReturn = await _transactionsService.GetTransactions(transactionKinds, startDate, endDate, page.Value, pageSize.Value, sortBy, sortOrder);

            return Ok(listToReturn);
        }
    }
}