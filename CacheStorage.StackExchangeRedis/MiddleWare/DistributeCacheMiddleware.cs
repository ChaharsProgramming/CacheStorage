using CacheStorage.StackExchangeRedis.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CacheStorage.StackExchangeRedis.MiddleWare
{
    public class DistributeCacheMiddleware : IMiddleware
    {
        private readonly IRedisDestributeCache redisDestributeCache;
        private readonly string redisPrefix = "dataContext_";

        public DistributeCacheMiddleware(IRedisDestributeCache cache)
        {
            redisDestributeCache = cache;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var isRequestMethodGet = context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase);
            string authToken = "";
            var path = context.Request.Path.ToString() + context.Request.QueryString.ToString();
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                authToken = context.Request.Headers["Authorization"];
            }

            string itemCache = null;
            if (isRequestMethodGet)
            {
                var tenantId = context.Request.Headers["tenantId"].ToString();
                if (redisDestributeCache.TryGetString("cachedTimeUTC", out string cacheItem))
                {

                    itemCache = cacheItem;
                    //context.Response.StatusCode = 304;
                    //await WriteResponseAsync(context, cacheItem);
                    if (!context.Response.HasStarted)
                    {
                        context.Response.Headers.Clear();
                        await context.Response.WriteAsync(cacheItem);
                    }
                    //context.Result = new StatusCodeResult((int)HttpStatusCode.NotModified);
                }
                else
                {
                    //await redisDestributeCache.SetStringAsync("cachedTimeUTC", tenantId);
                }

                //context.Response.Headers.Add("cache-control", new[] { "no-transform", "must-revalidate", "max-age=0" });
                //context.Response.Headers.Add("vary", new[] { "Accept", "Accept-Encoding", "Authorization", "Cookie" });
            }
            if (itemCache == null)
            {
                
                await next(context);
            }
        }

        private Task WriteResponseAsync(HttpContext context, string response= "")
        {
            
            //context.Response.ContentType = "application/json";
            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(response);
        }

        //public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    var isRequestMethodGet = context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase);
        //    string authToken = "";
        //    var path = context.Request.Path.ToString() + context.Request.QueryString.ToString();
        //    if (context.Request.Headers.ContainsKey("Authorization"))
        //    {
        //        authToken = context.Request.Headers["Authorization"];
        //    }

        //    string itemCache = string.Empty;
        //    if (isRequestMethodGet)
        //    {
        //        var tenantId = context.Request.Headers["tenantId"].ToString();
        //        if (redisDestributeCache.TryGetString("cachedTimeUTC", out string cacheItem))
        //        {

        //            itemCache = cacheItem;
        //            context.Response.StatusCode = 304;
        //            context.Result = new StatusCodeResult((int)HttpStatusCode.NotModified);
        //        }
        //        else
        //        {
        //            //await redisDestributeCache.SetStringAsync("cachedTimeUTC", tenantId);
        //        }

        //        context.Response.Headers.Add("cache-control", new[] { "no-transform", "must-revalidate", "max-age=0" });
        //        context.Response.Headers.Add("vary", new[] { "Accept", "Accept-Encoding", "Authorization", "Cookie" });
        //    }
        //    if (itemCache == null)
        //    {

        //        await base.OnActionExecutionAsync(context, next);
        //    }
        //}
    }
}
