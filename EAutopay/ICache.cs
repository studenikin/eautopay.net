namespace EAutopay
{
    /// <summary>
    /// Encapsulates caching capabilities.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Gets the cache item at the specified key.
        /// </summary>
        /// <param name="key">The key for the cache item.</param>
        /// <returns>A stored object.</returns>
        object Get(string key);

        /// <summary>
        /// Inserts an item into the cache.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="data">The object to be inserted into the cache.</param>
        void Set(string key, object data);
    }
}
