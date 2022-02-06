using DataStorage.Api.Services.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStorage.Api.Services
{
    public class SimpleStorageService : IStorageService
    {
        private readonly IDictionary<string, object> _storage;

        public SimpleStorageService()
        {
            _storage = new ConcurrentDictionary<string, object>();
        }

        public async Task PutAsync<T>(string key, object data)
        {
            await Task.CompletedTask;
            _storage[key] = data;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            await Task.CompletedTask;
            return (T)_storage[key];
        }

        public async Task<bool> DeleteAsync<T>(string key)
        {
            await Task.CompletedTask;
            return _storage.Remove(key);
        }
    }
}