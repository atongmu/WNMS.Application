using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WNMS.Application.Utility.Cache
{
    public class MemoryCacheExtensions : ICache
    {
        private readonly IMemoryCache _cache;
        /// <summary>
        /// 缓存配置项
        /// </summary>
        private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;
        //构造器注入
        public MemoryCacheExtensions(IMemoryCache cache)
        {
            //单项缓存设置项
            _memoryCacheEntryOptions = new MemoryCacheEntryOptions()
            {
                Priority = CacheItemPriority.Low,
                //缓存大小占1份
                Size = 1
            };
            _cache = cache;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheMin"></param>
        /// <param name="obsloteType"></param>
        public void Add<T>(string key, T data, ObsloteType obsloteType = default, int cacheMin = 120)
        {
            if (obsloteType == ObsloteType.Absolutely)
            {
                _memoryCacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(cacheMin);
            }
            if (obsloteType == ObsloteType.Relative)
            {
                _memoryCacheEntryOptions.SlidingExpiration = TimeSpan.FromMinutes(cacheMin);
            }
            if (Contains(key))
            {
                Upate<T>(key, data, obsloteType);
            }
            else
            {
                _cache.Set(key, data, _memoryCacheEntryOptions);
            }

        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            object RetValue;
            return _cache.TryGetValue(key, out RetValue);
        }
        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public bool Remove(string key)
        {
            bool ReturnBool = true;
            if (!Contains(key))
            {
                ReturnBool = false;
            }
            else
            {
                _cache.Remove(key);
            }

            return ReturnBool;
        }
        public bool Upate<T>(string key, T data, ObsloteType obsloteType, int cacheMin = 120)
        {
            bool ReturnBool = true;
            if (!Contains(key))
            {
                ReturnBool = false;
            }
            else
            {
                _cache.Remove(key);
                Add(key, data, obsloteType, cacheMin);
            }
            return ReturnBool;

        }
    }
}
