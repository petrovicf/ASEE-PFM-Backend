using TinyCsvParser.Mapping;
using Transactions.Mappings.Entities;

namespace Transactions.Mappings{
    public class CsvCategoryMapping : CsvMapping<CategoryCsv>{
        public CsvCategoryMapping():base(){
            MapProperty(0, c=>c.Code);
            MapProperty(1, c=>c.ParentCode);
            MapProperty(2, c=>c.Name);
        }
    }
}