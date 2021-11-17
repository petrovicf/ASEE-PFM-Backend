using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TinyCsvParser.Mapping;
using Transactions.Mappings.Entities;
using Transactions.Models.Transaction.Enums;
using Transactions.Problems;

namespace Transactions.Validation{
    public class Validate{
        public string PropertyName { get; set; }
        public bool Required { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public string Pattern { get; set; }
        public bool IsNumber { get; set; }
        public bool IsEnum { get; set; }

        public static ValidationProblem ValidateList<T>(List<T> list){
            List<Errors> errors = new List<Errors>();

            if(typeof(T) == typeof(CsvMappingResult<TransactionCsvEntity>)){
                errors = ValidateCsvMappingResultListTransactions(list as List<CsvMappingResult<TransactionCsvEntity>>);
            }
            else if(typeof(T) == typeof(CsvMappingResult<CategoryCsv>)){
                errors = ValidateCsvMappingResultListCategories(list as List<CsvMappingResult<CategoryCsv>>);
            }

            return new ValidationProblem{
                Errors = errors
            };
        }

        private static List<Errors> ValidateCsvMappingResultListTransactions(List<CsvMappingResult<TransactionCsvEntity>> list){
                List<Errors> errors = new List<Errors>();
                List<Validate> validations = new List<Validate>(){
                    {new Validate{PropertyName = "Result.Id", Required = true}},
                    {new Validate{PropertyName = "Result.Date", Required = true}},
                    {new Validate{PropertyName = "Result.Direction", Required = true, IsEnum = true}},
                    {new Validate{PropertyName = "Result.Amount", Required = true, Pattern = @"\d+.\d+", IsNumber=true}},
                    {new Validate{PropertyName = "Result.Currency", Required = true, MinLength = 3, MaxLength = 3}},
                    {new Validate{PropertyName = "Result.Mcc", Required = false, IsEnum = true}},
                    {new Validate{PropertyName = "Result.Kind", Required = true, IsEnum = true}}
                };
                string value;
                DateTime pomDate;
                ErrEnum err;
                double pomDouble;

                foreach(var item in list){
                    foreach(var property in validations){
                        value = GetPropertyValue(item, property.PropertyName).ToString();
                        if(property.Required){
                            if(string.IsNullOrEmpty(value.ToString())){
                                err = ErrEnum.Required;
                                errors.Add(CreateError(property.PropertyName.Split('.')[1], err, GetEnumDescription(err)));
                                break;
                            }
                        }
                        if(property.PropertyName.ToLower().Contains("date")){
                            if(!DateTime.TryParse(value, out pomDate)){
                                err = ErrEnum.InvalidFormat;
                                errors.Add(CreateError(property.PropertyName.Split('.')[1], err, GetEnumDescription(err)));
                                break;
                            }
                        }
                        if((property.MinLength > 0 && property.MaxLength > 0)){
                            if(value.Length < property.MinLength){
                                err = ErrEnum.MinLength;
                                errors.Add(CreateError(property.PropertyName.Split('.')[1], err, GetEnumDescription(err)));
                                break;
                            }
                            else if (value.Length > property.MinLength){
                                err = ErrEnum.MaxLength;
                                errors.Add(CreateError(property.PropertyName.Split('.')[1], err, GetEnumDescription(err)));
                                break;
                            }
                        }
                        if(property.IsNumber){
                            if(!double.TryParse(Regex.Match(value, property.Pattern).Value, out pomDouble)){
                                err = ErrEnum.InvalidFormat;
                                errors.Add(CreateError(property.PropertyName.Split('.')[1], err, GetEnumDescription(err)));
                                break;
                            }
                        }
                        if(property.IsEnum && value.Length > 0){
                            if(!TryParseEnums(value)){
                                err = ErrEnum.UnknownEnum;
                                errors.Add(CreateError(property.PropertyName.Split('.')[1], err, GetEnumDescription(err)));
                                break;
                            }
                        }
                    }
                }
                return errors;
        }

        private static List<Errors> ValidateCsvMappingResultListCategories(List<CsvMappingResult<CategoryCsv>> list){
                List<Errors> errors = new List<Errors>();
                
                List<Validate> validations = new List<Validate>(){
                    {new Validate{PropertyName = "Result.Code", Required = true}},
                    {new Validate{PropertyName = "Result.Name", Required = true}}
                };
                string value;
                ErrEnum err;

                foreach(var item in list){
                    foreach(var property in validations){
                        value = GetPropertyValue(item, property.PropertyName).ToString();
                        if(property.Required){
                            if(string.IsNullOrEmpty(value.ToString())){
                                err = ErrEnum.Required;
                                errors.Add(CreateError(property.PropertyName.Split('.')[1], err, GetEnumDescription(err)));
                                break;
                            }
                        }
                    }
                }

                return errors;
        }

        private static bool TryParseEnums(string enumeration){
            object en;
            return Enum.TryParse(typeof(DirectionsEnum), enumeration, true, out en) || Enum.TryParse(typeof(MccCodeEnum), enumeration, true, out en) 
            || Enum.TryParse(typeof(TransactionKindsEnum), enumeration, true, out en);
        }

        private static Errors CreateError(string tag, ErrEnum err, string message){
            return new Errors{
                Tag = tag,
                Error = err,
                Message = message
            };
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        private static object GetPropertyValue(object src, string propName)
        {
            if(propName.Contains("."))//complex type nested
            {
                var temp = propName.Split(new char[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);
                return prop.GetValue(src, null);
            }
        }
    }
}