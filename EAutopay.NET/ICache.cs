
namespace EAutopay.NET
{
    public interface ICache
    {
        object Get(string key);
        void Set(string key, object data);
    }
}
