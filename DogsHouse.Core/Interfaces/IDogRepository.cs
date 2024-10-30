using DogsHouse.Core.Models;
using DogsHouse.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouse.Core.Interfaces
{
    public interface IDogRepository
    {
        Task<IEnumerable<Dog>> GetDogsAsync(QueryParameters parameters);
        Task<Dog?> GetDogByNameAsync(string name);
        Task<Dog> AddDogAsync(Dog dog);
        Task<int> GetTotalCountAsync();
    }
}
