using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Transactions.Commands;
using Transactions.Database.Entities;
using Transactions.Database.Repositories;
using Transactions.Models.Transaction;
using Transactions.Models.Transaction.Enums;
using Transactions.Problems;

namespace Transactions.Services{
    public class TransactionsService : ITransactionsService{
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IMapper _mapper;

        public TransactionsService(ITransactionsRepository transactionsRepository, IMapper mapper){
            _transactionsRepository=transactionsRepository;
            _mapper=mapper;
        }

        public async Task<Problem> CategorizeTransaction(string id, TransactionCategorizeCommand transactionCategorizeCommand)
        {
            return await _transactionsRepository.Categorize(id, transactionCategorizeCommand);;
        }

        public async Task<TransactionPagedList<TransactionWithSplits>> GetTransactions(List<TransactionKindsEnum> transactionKinds = null, DateTime? startDate=null, DateTime? endDate = null, int page = 1,
        int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc)
        {
            var pagedList = await _transactionsRepository.Get(transactionKinds, startDate, endDate, page, pageSize, sortBy, sortOrder);

            return _mapper.Map<TransactionPagedList<TransactionWithSplits>>(pagedList);
        }

        public async Task<int> InsertTransactions(List<Transaction> transactions)
        {
            List<TransactionEntity> transToInsert = _mapper.Map<List<Models.Transaction.Transaction>, List<TransactionEntity>>(transactions);

            await _transactionsRepository.Insert(transToInsert);
            
            return 0;
        }

        public async Task<Problem> SplitTransaction(string id, SplitTransactionCommand splitTransactionCommand)
        {
            return await _transactionsRepository.Split(id, splitTransactionCommand);
        }
    }
}