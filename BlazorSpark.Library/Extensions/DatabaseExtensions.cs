using BlazorSpark.Library.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Extensions
{
    public static class DatabaseExtensions
    {
        public static IEnumerable<ActiveRecord> Save(this IEnumerable<ActiveRecord>? source)
        {
            source.Save();
            return source;
        }
    }
}
