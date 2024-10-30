using DogsHouse.Core.Interfaces;
using DogsHouse.Core.Models;
using DogsHouse.Core.Parameters;
using DogsHouse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DogsHouse.Infrastructure.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly DogsContext _context;

        public DogRepository(DogsContext context)
        {
            _context = context;
        }

        public async Task<Dog> AddDogAsync(Dog dog)
        {
            _context.Dogs.Add(dog);
            await _context.SaveChangesAsync();
            return dog;
        }

        public async Task<Dog?> GetDogByNameAsync(string name)
        {
            return await _context.Dogs.FirstOrDefaultAsync(d => d.Name == name);
        }

        public async Task<IEnumerable<Dog>> GetDogsAsync(QueryParameters parameters)
        {
            var query = _context.Dogs.AsQueryable();

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

            return await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Dogs.CountAsync();
        }
    }
}
