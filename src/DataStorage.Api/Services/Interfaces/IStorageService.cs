using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace DataStorage.Api.Services.Interfaces
{
    /// <summary>
    /// An interface for interacting with an object/binary data storage service.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Stores <paramref name="data"/> under the given <paramref name="key"/> in <paramref name="repository"/>.
        /// </summary>
        /// <param name="repository">The repository in which we are trying to store <paramref name="data"/>.</param>
        /// <param name="key">The key under which we are trying to store <paramref name="data"/>.</param>
        /// <param name="data">The data to be stored.</param>
        public void Put(string repository, Guid key, byte[] data);

        /// <summary>
        /// Retrieves the <paramref name="data"/> associated with the given <paramref name="key"/> in <paramref name="repository"/>.
        /// </summary>
        /// <param name="repository">The repository in which we are searching.</param>
        /// <param name="key">The key of the object/data that we are attempting to retrieve.</param>
        /// <param name="data">The retrieved data.</param>
        /// <returns><c>true</c> if the repository and key combination is valid, else <c>false</c>.</returns>
        public bool TryGetValue(string repository, Guid key, out byte[] data);

        /// <summary>
        /// Attempts to delete the data associated the given <paramref name="repository"/> and <paramref name="key"/>.
        /// </summary>
        /// <param name="repository">The repository in which we are searching.</param>
        /// <param name="key">The key of the object/data that we are attempting to retrieve.</param>
        /// <returns><c>true</c> if data was successfully removed from storage, else false if the repository and key combination is invalid.</returns>
        public bool Delete(string repository, Guid key);

        public IEnumerable<byte[]> GetRepository(string repository);
    }
}