using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CacheStorage.StackExchangeRedis.Db
{
    public class DataContextDbContext : DbContext
    {
        public DbSet<DataContext> DataContext { get; set; }

        public DataContextDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
