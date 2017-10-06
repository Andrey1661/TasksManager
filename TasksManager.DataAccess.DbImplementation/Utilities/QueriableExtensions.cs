using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TasksManager.DataAccess.DbImplementation.Utilities
{
    internal static class QueriableExtensions
    {
        internal static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IEnumerable<string> options)
        {
            var expression = source.Expression;
            var parameter = Expression.Parameter(typeof(T));
            int count = 0;

            foreach (var option in options)
            {
                SortModel model = SortModel.CreateFromString(option);
                var selector = Expression.PropertyOrField(parameter, model.Property);

                string method = count == 0 ? "OrderBy" : "ThenBy";
                if (model.Descending) method += "Descending";

                var lambda = Expression.Lambda(selector, parameter);
                var quote = Expression.Quote(lambda);

                expression = Expression.Call(typeof(Queryable), method,
                    new[] {source.ElementType, selector.Type}, expression, quote);

                count++;
            }

            return count == 0 ? source : source.Provider.CreateQuery<T>(expression);
        }
    }
}
