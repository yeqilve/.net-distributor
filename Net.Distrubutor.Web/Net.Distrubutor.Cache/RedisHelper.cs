using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Net.Distrubutor.Cache
{
    public class RedisHelper: IRedisHelper
    {
        private static ICache _cache;

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(_cache.GetRedisConnectionStr());
        });

        private static IDatabase _db;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        public RedisHelper(ICache cache)
        {
            _cache = cache;
        }

        public IDatabase DB
        {
            get {
                if (_db == null) {
                    _db = Connection.GetDatabase();
                }
                return _db;
            }           
        }

        public string Get(string key)
        {
            var serializedItem = DB.StringGetAsync(key);
            return serializedItem.ToString();
        }

        public bool Set(string key, string value)
        {
            return DB.SetAdd(key, value);
        }

        public bool Remove(string key)
        {
            return DB.KeyDelete(key);
        }

        public async void RemoveAsync(string key)
        {
            await DB.KeyDeleteAsync(key);
        }
    }
}
