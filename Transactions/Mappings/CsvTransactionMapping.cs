using System.ComponentModel;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;
using Transactions.Database.Entities;
using Transactions.Mappings.Entities;
using Transactions.Models.Transaction.Enums;

namespace Transactions.Mappings{
    public class CsvTransactionMapping : CsvMapping<TransactionCsvEntity>{
        public CsvTransactionMapping() : base(){
            MapProperty(0, x=>x.Id);
            MapProperty(1, x=>x.BeneficiaryName);
            MapProperty(2, x=>x.Date);
            MapProperty(3, x=>x.Direction);
            MapProperty(4, x=>x.Amount);
            MapProperty(5, x=>x.Description);
            MapProperty(6, x=>x.Currency);
            MapProperty(7, x=>x.Mcc);
            MapProperty(8, x=>x.Kind);
        }
    }
}