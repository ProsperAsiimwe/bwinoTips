using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace BwinoTips.WebUI.Models
{
    public class FilterModel
    {
        public FilterModel()
        {
            Page = 1;
        }

        public FilterModel(string defaultSort) : this()
        {
            Sort = defaultSort;
        }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public string Sort { get; set; }

        public string SortDir { get; set; }

        public int SkipValue()
        {
            return ((Page - 1) * PageSize);
        }

        public int ItemsFrom()
        {
            return (((Page - 1) * PageSize) + 1);
        }

        public int ItemsTo()
        {
            int total = (((ItemsFrom() + PageSize) - 1) > TotalItems) ? TotalItems : ((ItemsFrom() + PageSize) - 1);

            return total;
        }

        public FilterModel Init(string defaultSort, int pageSize)
        {
            if (string.IsNullOrEmpty(Sort))
            {
                Sort = defaultSort;
            }

            PageSize = pageSize;

            return this;
        }

        public FilterModel Init(string defaultSort, string defaultSortDir, int pageSize)
        {
            var model = Init(defaultSort, pageSize);

            if (string.IsNullOrEmpty(SortDir))
            {
                SortDir = defaultSortDir;
            }

            return this;
        }

        public Func<TSource, object> OrderBy<TSource>(string propertyName)
        {
            //this will return p=>p.<< your proeprty name >>
            var type = typeof(TSource);
            var parameter = Expression.Parameter(type, "p");
            var propertyReference = Expression.Property(parameter, propertyName);
            return Expression.Lambda<Func<TSource, object>>(propertyReference, new[] { parameter }).Compile();
        }

        public Func<T, object> GetPropertyLambda<T>(string propertyName)
        {
            PropertyInfo property = typeof(T).GetProperty(propertyName);
            return p => property.GetValue(p, null);
        }

        public int[] GetRowsPerPage()
        {
            return new[] { 5, 10, 20 };
        }
    }
}