using CacheStorage.StackExchangeRedis.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CacheStorage.StackExchangeRedis.Provider
{
    public class DataContextProvider : IDataContextProvider
    {
        private readonly DataContextDbContext myDbContext;
        private readonly IDistributedCache myDistributedCache;
        private string dataLocation;

        public DataContextProvider(DataContextDbContext dbContext, IDistributedCache distributedCache)

        {
            //myDistributedCache = distributedCache;
            myDbContext = dbContext;
            SeedData();
        }

        private void SeedData()
        {
            if (!myDbContext.DataContext.Any())
            {
                myDbContext.DataContext.Add(new Db.DataContext() { Id = 1, Name = "abc1", CreatedBy = "Institute1", CreationTime = DateTime.Now, ModificationTime = DateTime.UtcNow, Properties = "Hospital", TenantId = 1 });
                myDbContext.DataContext.Add(new Db.DataContext() { Id = 2, Name = "abc1", CreatedBy = "Institute2", CreationTime = DateTime.Now, ModificationTime = DateTime.UtcNow, Properties = "Clinic", TenantId = 2 });
                myDbContext.SaveChanges();
            }
        }

        public async Task<List<DataContext>> GetDataContextAsync()
        {
            try
            {
                List<DataContext> dataContexts = null;
                //ResponseModel responseModel = new ResponseModel();

                //var recordkey = "DataContext_" + DateTime.Now.ToString("yyyyMMdd_hhmm");

                //var dataContextFromRedis = await GetRecordAsync<List<DataContext>>(myDistributedCache, recordkey);

                //if (dataContextFromRedis is null)
                //{
                    dataContexts = await myDbContext.DataContext.ToListAsync();
                //await Task.Delay(2000);
                //dataLocation = "From Api";
                //responseModel = new ResponseModel { DataContexts = dataContexts, Location = dataLocation };
                //await SetRecordAsync(myDistributedCache, recordkey, dataContexts);
                if (dataContexts is not null)
                {
                    return dataContexts;
                }
                return (null);
                //}
                //else
                //{
                //dataLocation = "From Cache";
                //    responseModel = new ResponseModel { DataContexts = dataContextFromRedis, Location = dataLocation };
                    
                //}
            }
            catch (Exception ex)
            {
                return (null);
            }
        }

        //private async Task SetRecordAsync<T>(IDistributedCache cache,
        //    string recordId,
        //    T dataContext, TimeSpan? absoluteExpireTime = null,
        //    TimeSpan? unusualExpiredTime = null)
        //{
        //    var options = new DistributedCacheEntryOptions();
        //    options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
        //    options.SlidingExpiration = unusualExpiredTime;


        //    var jsonData = JsonSerializer.Serialize(dataContext);
        //    await cache.SetStringAsync(recordId, jsonData, options);
        //}

        //private async Task<T> GetRecordAsync<T>(IDistributedCache cache,
        //    string recordId)
        //{
        //    var jsonData = await cache.GetStringAsync(recordId);
        //    if (jsonData is null)
        //    {
        //        return default(T);
        //    }
        //    return JsonSerializer.Deserialize<T>(jsonData);
        //}

        //private async Task RemoveAsync<T>(IDistributedCache cache,
        //   string recordId)
        //{
        //    await cache.RemoveAsync(recordId);
        //}

        //private async Task RefreshAsync<T>(IDistributedCache cache,
        //   string recordId)
        //{
        //    await cache.RefreshAsync(recordId);
        //}
    }
}
