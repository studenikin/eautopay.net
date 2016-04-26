using System.Web;

namespace EAutopay
{
    public class HttpCache : ICache
    {
        public object Get(string key)
        {
            return HttpContext.Current.Cache[key];
        }

        public void Set(string key, object data)
        {
            HttpContext.Current.Cache.Insert(key, data);
        }
    }
}
