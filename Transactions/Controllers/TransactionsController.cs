using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            var transactions = _mapper.Map<List<CsvMappingResult<TransactionCsvEntity>>, List<Models.Transaction.Transaction>>(transactionList);

            transactions.RemoveAt(transactions.Count-1);
            await _transactionsService.InsertTransactions(transactions);

            System.IO.File.Delete(filePath);

            return Ok("Transaction splitted");
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