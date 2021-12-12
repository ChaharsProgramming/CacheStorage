using CacheStorage.StackExchangeRedis.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CacheStorage.StackExchangeRedis.Provider
{
    public interface IDataContextProvider
    {
        public Task<List<DataContext>> GetDataContextAsync();
    }
}
