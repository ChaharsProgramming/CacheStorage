//using CacheStorage.StackExchangeRedis.Provider;
using CacheStorage.StackExchangeRedis.Db;
using CacheStorage.StackExchangeRedis.Provider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleStorage.CacheRedis.Controllers
{
    [ApiController]
    [Route("api/dataContext")]
    public class DataContextController : ControllerBase
    {
        private readonly IDataContextProvider myDataContextProvider;
        private readonly IDistributedCache myDistributedCache;
        public DataContextController(IDistributedCache distributedCache, IDataContextProvider dataContextProvider)
        {
            myDataContextProvider = dataContextProvider;
            myDistributedCache = distributedCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDataContextAsync()
        {
            var response = new ResponseModel();
            response.DataContexts = await myDataContextProvider.GetDataContextAsync();
            var jsonData = JsonSerializer.Serialize(response.DataContexts);
            await myDistributedCache.SetStringAsync("cachedTimeUTC", jsonData);

            ////var dataContextFromRedis = await myDistributedCache.GetStringAsync("cachedTimeUTC");
            ////var output = JsonSerializer.Deserialize<ResponseModel>(dataContextFromRedis);
            ////if (dataContextFromRedis is null)
            ////{

            ////    //var jsonData = JsonSerializer.Serialize(response.DataContexts);
            ////    //await myDistributedCache.SetStringAsync("cachedTimeUTC", jsonData);
            ////    response.Location = "From Api";
            ////}
            //response.DataContexts = output.DataContexts;
            //response.Location = "From Cache";
            if (response.DataContexts != null)
            {
                return Ok(response.DataContexts);
            }
            return NotFound();
        }






    }
}
