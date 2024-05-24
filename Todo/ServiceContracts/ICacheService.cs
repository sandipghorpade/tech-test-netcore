using System.Threading.Tasks;
using System;

namespace Todo.ServiceContracts
{
    public interface ICacheService
    {
        void Set<T>(string key, T value);
        bool Get<T>(string key, out T value);  
        void Remove(string key);
    }
}
