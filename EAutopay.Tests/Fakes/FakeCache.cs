using System.Collections.Generic;

namespace EAutopay.Tests.Fakes
{
    class FakeCache : ICache
    {
        private Dictionary<string, object> _cache;

        public FakeCache()
        {
            _cache = new Dictionary<string, object>();
        }

        public object Get(string key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }
            return null;
        }

        public void Set(string key, object data)
        {
            if (_cache.ContainsKey(key))
            {
                _cache[key] = data;
            }
            else
            {
                _cache.Add(key, data);
            }
        }
    }
}
