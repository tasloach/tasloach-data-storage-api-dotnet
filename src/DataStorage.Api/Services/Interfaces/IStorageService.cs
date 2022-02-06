using System.Threading.Tasks;

namespace DataStorage.Api.Services.Interfaces
{
    /// <summary>
    /// An interface for a storage service.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Stores the given <paramref name="data"/> using the given <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key under which the data is being stored.</param>
        /// <param name="data">The data that's being stored in the service.</param>
        public Task PutAsync<T>(string key, object data);

        /// <summary>
        /// A method that attempts to retrieve the data associated with the given key from the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>A task representing the data/object stored under the given key.</returns>
        public Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Removes the data/object under <paramref name="key"/> in the dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>A task representing the completion of the delete.</returns>
        public Task<bool> DeleteAsync<T>(string key);
    }
}