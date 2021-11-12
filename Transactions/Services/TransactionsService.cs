using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Transactions.Database.Entities;
using Transactions.Database.Repositories;
using Transactions.Models.Transaction;

namespace Transactions.Services{
    public class TransactionsService : ITransactionsService{
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IMapper _mapper;

        public TransactionsService(ITransactionsRepository transactionsRepository, IMapper mapper){
            _transactionsRepository=transactionsRepository;
            _mapper=mapper;
        }

        public async Task<int> InsertTransactions(List<Transaction> transactions)
        {
            List<TransactionEntity> transToInsert = _mapper.Map<List<Models.Transaction.Transaction>, List<TransactionEntity>>(transactions);

            await _transactionsRepository.Insert(transToInsert);

            return 0;
        }
    }
}