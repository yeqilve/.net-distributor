using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Net.Distrubutor.Cache
{
    public interface IRedisHelper
    {
        string Get(string key);
        bool Set(string key, string value);
        bool Remove(string key);
        void RemoveAsync(string key);
    }
}
