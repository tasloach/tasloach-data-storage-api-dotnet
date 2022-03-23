using DataStorage.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace DataStorage.Api.Services
{
    /// <inheritdoc cref="IStorageService"/>
    public class SimpleStorageService : IStorageService
    {
        private readonly IDictionary<string, IDictionary<Guid, byte[]>> _storage;

        public SimpleStorageService()
        {
            _storage = new Dictionary<string, IDictionary<Guid, byte[]>>();
        }

        public void Put(string repository, Guid key, byte[] data)
        {
            if (_storage.ContainsKey(repository))
            {
                _storage[repository][key] = data;
            }
            else
            {
                _storage[repository] = new Dictionary<Guid, byte[]> { { key, data } };
            }
        }

        public bool TryGetValue(string repository, Guid key, out byte[] data)
        {
            if (!_storage.ContainsKey(repository) || !_storage[repository].ContainsKey(key))
            {
                data = null;
                return false;
            }

            data = _storage[repository][key];
            return true;
        }

        public bool Delete(string repository, Guid key)
        {
            if (!_storage.ContainsKey(repository) || !_storage[repository].ContainsKey(key))
                return false;

            return _storage[repository].Remove(key);
        }

        public IEnumerable<byte[]> GetRepository(string repository)
        {
            if (!_storage.ContainsKey(repository))
                return Enumerable.Empty<byte[]>();

            return _storage[repository].Select(x => x.Value);
        }
    }
}