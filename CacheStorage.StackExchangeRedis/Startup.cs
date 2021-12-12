using CacheStorage.StackExchangeRedis.Db;
using CacheStorage.StackExchangeRedis.Helper;
using CacheStorage.StackExchangeRedis.MiddleWare;
using CacheStorage.StackExchangeRedis.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace CacheStorage.StackExchangeRedis
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataContextProvider, DataContextProvider>();
            services.AddScoped(typeof(Startup));
            services.AddScoped<IRedisDestributeCache, DistributeCache>();
            services.AddScoped<DistributeCacheMiddleware>();
            //services.addn
            services.AddDbContext<DataContextDbContext>(options =>
            {
                options.UseInMemoryDatabase("DataContextInMemory");
            });
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "127.0.0.1:6379";
                options.InstanceName = "DataContext_";
            });
            //services.AddStackExchangeRedisCache
            

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SampleStorage.CacheRedis", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            IHostApplicationLifetime hostApplicationLifetime, IDistributedCache distributedCache)
        {
            hostApplicationLifetime.ApplicationStarted.Register(() =>
            {
                var key = "DataContext_" + DateTime.Now.ToString("yyyyMMdd_hhmm");
                byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(key);

                var options = new DistributedCacheEntryOptions();
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3000);
                options.SlidingExpiration = null;

                //distributedCache.Set("cachedTimeUTC", encodedCurrentTimeUTC, options);
                
            });

            if (env.IsDevelopment())
            {
                //app.UseMiddleware(typeof(DistributeCacheMiddleware));
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SampleStorage.CacheRedis v1"));

                app.UseMiddleware<DistributeCacheMiddleware>();
                //app.UseMiddleware(typeof(DistributeCacheMiddleware));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
