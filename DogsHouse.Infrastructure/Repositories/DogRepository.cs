using DogsHouse.Core.Interfaces;
using DogsHouse.Core.Models;
using DogsHouse.Core.Parameters;
using DogsHouse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DogsHouse.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IDogRepository using Entity Framework Core
    /// </summary>
    public class DogRepository : IDogRepository
    {
        private readonly DogsContext _context;

        /// <summary>
        /// Initializes a new instance of the DogRepository
        /// </summary>
        /// <param name="context">Database context</param>
        public DogRepository(DogsContext context)
        {
            _context = context;
        }
        /// <inheritdoc/>
        public async Task<Dog> AddDogAsync(Dog dog)
        {
            _context.Dogs.Add(dog);
            await _context.SaveChangesAsync();
            return dog;
        }

        /// <inheritdoc/>
        public async Task<Dog?> GetDogByNameAsync(string name)
        {
            return await _context.Dogs.FirstOrDefaultAsync(d => d.Name == name);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Dog>> GetDogsAsync(QueryParameters parameters)
        {
            var query = _context.Dogs.AsQueryable();

            // Apply sorting if specified
            if (!string.IsNullOrWhiteSpace(parameters.Attribute) && !string.IsNullOrWhiteSpace(parameters.Order))
            {
                query = parameters.Attribute.ToLower() switch
                {
                    "name" => parameters.Order.ToLower() == "desc" ?
                        query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
                    "weight" => parameters.Order.ToLower() == "desc" ?
                        query.OrderByDescending(d => d.Weight) : query.OrderBy(d => d.Weight),
                    "tail_length" => parameters.Order.ToLower() == "desc" ?
                        query.OrderByDescending(d => d.TailLength) : query.OrderBy(d => d.TailLength),
                    _ => query
                };
            }

            // Apply pagination
            return await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Dogs.CountAsync();
        }
    }
}
