using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spark.Library.Database;

namespace Spark.Library.Extensions
{
    public static class DbSetExtensions
    {
        public static void Save<TEntity>(this DbSet<TEntity> table, TEntity o) where TEntity : BaseModel
        {
            var context = table.GetService<ICurrentDbContext>().Context;

            if (o.Id == 0)
            {
                // insert
                o.CreatedAt = DateTime.UtcNow;
                table.Add(o);
                context.SaveChanges();
            }
            else
            {
                // update
                o.UpdatedAt = DateTime.UtcNow;
                table.Update(o);
                context.SaveChanges();
            }
        }
        public static void Delete<TEntity>(this DbSet<TEntity> table, TEntity o) where TEntity : BaseModel
        {
            var context = table.GetService<ICurrentDbContext>().Context;
            table.Remove(o);
            context.SaveChanges();
        }
    }
}
