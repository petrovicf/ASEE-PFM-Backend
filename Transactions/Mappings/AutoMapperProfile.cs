using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Transactions;
using AutoMapper;
using TinyCsvParser.Mapping;
using Transactions.Database.Entities;
using Transactions.Mappings.Entities;
using Transactions.Models.Category;
using Transactions.Models.Transaction;
using Transactions.Models.Transaction.Enums;

namespace Transactions.Mappings{
    public class AutoMapperProfile : Profile{
        public AutoMapperProfile(){
            /*CreateMap<CsvMappingResult<TransactionCsvEntity>, TransactionCsvEntity>()
                .ForMember(d=>d.Id, mo=>mo.MapFrom(s=>s.Result.Id))
                .ForMember(d=>d.BeneficiaryName, mo=>mo.MapFrom(s=>s.Result.BeneficiaryName))
                .ForMember(d=>d.Date, mo=>mo.MapFrom(s=>s.Result.Date))
                .ForMember(d=>d.Direction, mo=>mo.MapFrom(s=>s.Result.Direction))
                .ForMember(d=>d.Amount, mo=>mo.MapFrom(s=>s.Result.Amount))
                .ForMember(d=>d.Description, mo=>mo.MapFrom(s=>s.Result.Description))
                .ForMember(d=>d.Currency, mo=>mo.MapFrom(s=>s.Result.Currency))
                .ForMember(d=>d.Mcc, mo=>mo.MapFrom(s=>s.Result.Mcc))
                .ForMember(d=>d.Kind, mo=>mo.MapFrom(s=>s.Result.Kind));

            CreateMap<TransactionCsvEntity, Models.Transaction.Transaction>().ForMember(d=>d.Amount, mo=>mo.MapFrom(s=>double.Parse(Regex.Match(s.Amount,@"\d+.\d+").Value)));*/

            CreateMap<CsvMappingResult<TransactionCsvEntity>, Models.Transaction.Transaction>()
                .ForMember(d=>d.Id, mo=>mo.MapFrom(s=>s.Result.Id))
                .ForMember(d=>d.BeneficiaryName, mo=>mo.MapFrom(s=>s.Result.BeneficiaryName))
                .ForMember(d=>d.Date, mo=>mo.MapFrom(s=>DateTime.Parse(s.Result.Date)))
                .ForMember(d=>d.Direction, mo=>mo.MapFrom(s=>Enum.Parse(typeof(DirectionsEnum), s.Result.Direction,true)))
                .ForMember(d=>d.Amount, mo=>mo.MapFrom(s=>double.Parse(Regex.Match(s.Result.Amount,@"\d+.\d+").Value)))
                .ForMember(d=>d.Description, mo=>mo.MapFrom(s=>s.Result.Description))
                .ForMember(d=>d.Currency, mo=>mo.MapFrom(s=>s.Result.Currency))
                .ForMember(d=>d.Mcc, mo=>mo.MapFrom(s=>s.Result.Mcc))
                .ForMember(d=>d.Kind, mo=>mo.MapFrom(s=>Enum.Parse(typeof(TransactionKindsEnum),s.Result.Kind,true)));

            CreateMap<Models.Transaction.Transaction, TransactionEntity>();
            //CreateMap<List<Models.Transaction.Transaction>, List<TransactionEntity>>();
            CreateMap<TransactionEntity, Models.Transaction.Transaction>();
            CreateMap<TransactionPagedList<TransactionEntity>, TransactionPagedList<Models.Transaction.Transaction>>();

            CreateMap<CsvMappingResult<CategoryCsv>, Category>()
                .ForMember(c=>c.Code, mo=>mo.MapFrom(cs=>cs.Result.Code))
                .ForMember(c=>c.ParentCode, mo=>mo.MapFrom(cs=>cs.Result.ParentCode))
                .ForMember(c=>c.Name, mo=>mo.MapFrom(cs=>cs.Result.Name));
            CreateMap<Category, CategoryEntity>();

            CreateMap<SingleCategorySplit, SplitEntity>();
            CreateMap<SplitEntity, SingleCategorySplit>();
            CreateMap<TransactionEntity, TransactionWithSplits>();
            CreateMap<TransactionPagedList<TransactionEntity>, TransactionPagedList<TransactionWithSplits>>();
        }
    }
}