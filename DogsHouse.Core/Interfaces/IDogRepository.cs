using DogsHouse.Core.Models;
using DogsHouse.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouse.Core.Interfaces
{
    /// <summary>
    /// Repository interface for dog data access operations
    /// </summary>
    public interface IDogRepository
    {
        /// <summary>
        /// Retrieves a paginated list of dogs based on query parameters
        /// </summary>
        /// <param name="parameters">Query parameters for filtering and pagination</param>
        /// <returns>Collection of dogs matching the query parameters</returns>
        Task<IEnumerable<Dog>> GetDogsAsync(QueryParameters parameters);
        /// <summary>
        /// Retrieves a dog by its name
        /// </summary>
        /// <param name="name">Name of the dog to retrieve</param>
        /// <returns>Dog if found, null otherwise</returns>
        Task<Dog?> GetDogByNameAsync(string name);
        /// <summary>
        /// Adds a new dog to the database
        /// </summary>
        /// <param name="dog">Dog entity to add</param>
        /// <returns>Added dog with generated Id</returns>
        Task<Dog> AddDogAsync(Dog dog);
        /// <summary>
        /// Gets the total count of dogs in the database
        /// </summary>
        /// <returns>Total number of dogs</returns>
        Task<int> GetTotalCountAsync();
    }
}
