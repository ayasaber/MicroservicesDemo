using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandFlags = StackExchange.Redis.CommandFlags;

namespace Caching.Redis
{
    public class CacheManager : ICacheManager
    {
        private StackExchange.Redis.IDatabase RedisCache;

        private readonly IConfiguration _configuration;
        public CacheManager(IConfiguration Configuration) 
        {
            _configuration = Configuration;
         
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            RedisCache = redis.GetDatabase();
       
        }

        public bool AddSet(string key, string value)
        {
            var result = false;

            try
            {
                var redis = RedisCache;
                redis.SetAdd(key, value);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
        public bool AddString(string key, string value)
        {
            var result = false;

            try
            {
                var redis = RedisCache;
                redis.StringSet(key, value);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
        public bool AddString(string key, string value, TimeSpan expiry)
        {
            var result = false;

            try
            {
                var redis = RedisCache;
                redis.StringSet(key, value, expiry);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
        public bool AddObject<T>(string key, T value) where T : class
        {
            return AddString(key, JsonConvert.SerializeObject(value));
        }
        public bool AddObject<T>(string key, T value, TimeSpan expiry) where T : class
        {
            return AddString(key, JsonConvert.SerializeObject(value), expiry);
        }
        public bool Delete(string key)
        {
            var result = false;

            try
            {
                var redis = RedisCache;
                redis.KeyDelete(key, CommandFlags.FireAndForget);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
        public string GetString(string key)
        {
            var result = string.Empty;

            try
            {
                var redis = RedisCache;
                result = redis.StringGet(key);
            }
            catch (Exception)
            {
                result = string.Empty;
            }

            return result;
        }
        public T GetObject<T>(string key) where T : class
        {
            string str = GetString(key);

            if (string.IsNullOrEmpty(str))
                return null;

            return JsonConvert.DeserializeObject<T>(str);
        }
    }

}
