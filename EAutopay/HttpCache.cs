using System.Web;

namespace EAutopay
{
    /// <summary>
    /// Provides caching capabilities based on System.Web.HttpContext.
    /// </summary>
    public class HttpCache : ICache
    {
        /// <summary>
        /// Gets the cache item at the specified key.
        /// </summary>
        /// <param name="key">The key for the cache item.</param>
        /// <returns>A stored object.</returns>
        public object Get(string key)
        {
            return HttpContext.Current.Cache[key];
        }

        /// <summary>
        /// Inserts an item into the cache.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="data">The object to be inserted into the cache.</param>
        public void Set(string key, object data)
        {
            HttpContext.Current.Cache.Insert(key, data);
        }
    }
}
