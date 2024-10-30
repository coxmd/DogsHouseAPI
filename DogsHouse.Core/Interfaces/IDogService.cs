using DogsHouse.Core.DTOs;
using DogsHouse.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouse.Core.Interfaces
{
    public interface IDogService
    {
        Task<IEnumerable<DogDto>> GetDogsAsync(QueryParameters parameters);
        Task<DogDto> CreateDogAsync(DogDto dogDto);
    }
}
