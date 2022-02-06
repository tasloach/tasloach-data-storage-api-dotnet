using System;

namespace DataStorage.Api.Services.Interfaces
{
    /// <summary>
    /// An interface for a storage service.
    /// </summary>
    public interface IStorageService
    {
        public void Put(string repository, Guid key, byte[] data);

        public bool TryGetValue(string repository, Guid key, out byte[] value);

        public bool Delete(string repository, Guid key);
    }
}