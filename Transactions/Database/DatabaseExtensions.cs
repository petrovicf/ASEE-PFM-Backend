using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Transactions.Database.Entities;

namespace Transactions.Database{
    public static class DatabaseExtensions{
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, string propertyName, Expression<Func<TSource, TKey>> defaultOrderingProperty){
            if(string.IsNullOrEmpty(propertyName)){
                return source.OrderBy(defaultOrderingProperty);
            }

            propertyName = UpperFirst(propertyName);

            if(typeof(TSource).GetProperty(propertyName)==null){
                return source.OrderBy(defaultOrderingProperty);
            }

            var parameter = Expression.Parameter(typeof(TSource), "x"); //x (as TSource)
            Expression property = Expression.Property(parameter, propertyName); //x.[propertyName]
            var lambda = Expression.Lambda(property, parameter);    //x=>x.[propertyName]

            var orderByMethod = typeof(Queryable).GetMethods().First(x=>x.Name=="OrderBy" && x.GetParameters().Length==2);  //OrderBy(param1, param2)?
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TSource), property.Type);   //OrderBy() for TSource type?
            var result = orderByGeneric.Invoke(null, new object[]{source, lambda}); //source.OrderBy(x=>x.[propertyName])

            return result as IOrderedQueryable<TSource>;
        }

        public static IOrderedQueryable<TSource> OrderByDescending<TSource, TKey>(this IQueryable<TSource> source, string propertyName, Expression<Func<TSource, TKey>> defaultOrderingProperty){
            if(string.IsNullOrEmpty(propertyName)){
                return source.OrderByDescending(defaultOrderingProperty);
            }

            propertyName = UpperFirst(propertyName);

            if(typeof(TSource).GetProperty(propertyName)==null){
                return source.OrderByDescending(defaultOrderingProperty);
            }

            var parameter = Expression.Parameter(typeof(TSource), "x"); //x (as TSource)
            Expression property = Expression.Property(parameter, propertyName); //x.[propertyName]
            var lambda = Expression.Lambda(property, parameter);    //x=>x.[propertyName]

            var orderByMethod = typeof(Queryable).GetMethods().First(x=>x.Name=="OrderByDescending" && x.GetParameters().Length==2);  //OrderBy(param1, param2)?
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TSource), property.Type);   //OrderBy() for TSource type?
            var result = orderByGeneric.Invoke(null, new object[]{source, lambda}); //source.OrderBy(x=>x.[propertyName])

            return result as IOrderedQueryable<TSource>;
        }

        public static IQueryable<T> SetSplits<T>(this IQueryable<T> queryable, List<SplitEntity> splits) where T:TransactionEntity{
            var list = queryable.ToList();
            list.ForEach(t=>t.Splits = splits.Where(s=>s.TransactionId==t.Id).ToList());

            return queryable;
        }

        private static string UpperFirst(string s){
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}