using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Transactions.Database.Entities;
using Transactions.Database.Repositories;
using Transactions.Models.Transaction;
using Transactions.Models.Transaction.Enums;

namespace Transactions.Services{
    public class TransactionsService : ITransactionsService{
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IMapper _mapper;

        public TransactionsService(ITransactionsRepository transactionsRepository, IMapper mapper){
            _transactionsRepository=transactionsRepository;
            _mapper=mapper;
        }

        public async Task<TransactionPagedList<Transaction>> GetTransactions(List<TransactionKindsEnum> transactionKinds = null, DateTime? startDate=null, DateTime? endDate = null, int page = 1,
        int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc)
        {
            var pagedList = await _transactionsRepository.Get(transactionKinds, startDate, endDate, page, pageSize, sortBy, sortOrder);

            return _mapper.Map<TransactionPagedList<Transaction>>(pagedList);
        }

        public async Task<int> InsertTransactions(List<Transaction> transactions)
        {
            List<TransactionEntity> transToInsert = _mapper.Map<List<Models.Transaction.Transaction>, List<TransactionEntity>>(transactions);

            await _transactionsRepository.Insert(transToInsert);
            
            return 0;
        }
    }
}