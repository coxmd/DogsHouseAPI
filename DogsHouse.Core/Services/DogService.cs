using AutoMapper;
using DogsHouse.Core.DTOs;
using DogsHouse.Core.Interfaces;
using DogsHouse.Core.Models;
using DogsHouse.Core.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouse.Core.Services
{
    public class DogService : IDogService
    {
        private readonly IDogRepository _repository;
        private readonly IMapper _mapper;

        public DogService(IDogRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DogDto> CreateDogAsync(DogDto createDogDto)
        {
            // Validation
            if (createDogDto.TailLength < 0)
                throw new ValidationException("Tail length cannot be negative");

            if (createDogDto.Weight <= 0)
                throw new ValidationException("Weight must be positive");

            var existingDog = await _repository.GetDogByNameAsync(createDogDto.Name);
            if (existingDog != null)
                throw new ValidationException("Dog with this name already exists");

            var dog = _mapper.Map<Dog>(createDogDto);
            var result = await _repository.AddDogAsync(dog);
            return _mapper.Map<DogDto>(result);
        }

        public async Task<IEnumerable<DogDto>> GetDogsAsync(QueryParameters parameters)
        {
            var dogs = await _repository.GetDogsAsync(parameters);
            return _mapper.Map<IEnumerable<DogDto>>(dogs);
        }
    }
}
