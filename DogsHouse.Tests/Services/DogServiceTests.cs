using AutoMapper;
using DogsHouse.Core.DTOs;
using DogsHouse.Core.Interfaces;
using DogsHouse.Core.Models;
using DogsHouse.Core.Parameters;
using DogsHouse.Core.Services;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace DogsHouse.Tests.Services
{
    public class DogServiceTests
    {
        private readonly Mock<IDogRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DogService _service;

        public DogServiceTests()
        {
            _mockRepo = new Mock<IDogRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new DogService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateDog_WithValidData_ReturnsSuccessfully()
        {
            // Arrange
            var createDto = new DogDto { Name = "Test", Color = "Brown", TailLength = 10, Weight = 20 };
            var dog = new Dog { Id = 1, Name = "Test", Color = "Brown", TailLength = 10, Weight = 20 };
            var dogDto = new DogDto { Name = "Test", Color = "Brown", TailLength = 10, Weight = 20 };

            _mockRepo.Setup(r => r.GetDogByNameAsync(It.IsAny<string>())).ReturnsAsync((Dog?)null);
            _mockRepo.Setup(r => r.AddDogAsync(It.IsAny<Dog>())).ReturnsAsync(dog);
            _mockMapper.Setup(m => m.Map<Dog>(It.IsAny<DogDto>())).Returns(dog);
            _mockMapper.Setup(m => m.Map<DogDto>(It.IsAny<Dog>())).Returns(dogDto);

            // Act
            var result = await _service.CreateDogAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createDto.Name, result.Name);
            Assert.Equal(createDto.Color, result.Color);
            Assert.Equal(createDto.TailLength, result.TailLength);
            Assert.Equal(createDto.Weight, result.Weight);
            _mockRepo.Verify(r => r.AddDogAsync(It.IsAny<Dog>()), Times.Once);
        }

        [Fact]
        public async Task CreateDog_WithNegativeTailLength_ThrowsValidationException()
        {
            // Arrange
            var createDto = new DogDto { Name = "Test", Color = "Brown", TailLength = -1, Weight = 20 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.CreateDogAsync(createDto));
            Assert.Equal("Tail length cannot be negative", exception.Message);
            _mockRepo.Verify(r => r.AddDogAsync(It.IsAny<Dog>()), Times.Never);
        }

        [Fact]
        public async Task CreateDog_WithZeroWeight_ThrowsValidationException()
        {
            // Arrange
            var createDto = new DogDto { Name = "Test", Color = "Brown", TailLength = 10, Weight = 0 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.CreateDogAsync(createDto));
            Assert.Equal("Weight must be positive", exception.Message);
            _mockRepo.Verify(r => r.AddDogAsync(It.IsAny<Dog>()), Times.Never);
        }

        [Fact]
        public async Task CreateDog_WithExistingName_ThrowsValidationException()
        {
            // Arrange
            var createDto = new DogDto { Name = "Test", Color = "Brown", TailLength = 10, Weight = 20 };
            var existingDog = new Dog { Id = 1, Name = "Test", Color = "Brown", TailLength = 10, Weight = 20 };

            _mockRepo.Setup(r => r.GetDogByNameAsync(It.IsAny<string>())).ReturnsAsync(existingDog);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.CreateDogAsync(createDto));
            Assert.Equal("Dog with this name already exists", exception.Message);
            _mockRepo.Verify(r => r.AddDogAsync(It.IsAny<Dog>()), Times.Never);
        }

        [Fact]
        public async Task GetDogs_ReturnsCorrectly()
        {
            // Arrange
            var parameters = new QueryParameters();
            var dogs = new List<Dog>
            {
                new() { Id = 1, Name = "Test1", Color = "Brown", TailLength = 10, Weight = 20 },
                new() { Id = 2, Name = "Test2", Color = "Black", TailLength = 15, Weight = 25 }
            };
            var dogsDto = dogs.Select(d => new DogDto
            {
                Name = d.Name,
                Color = d.Color,
                TailLength = d.TailLength,
                Weight = d.Weight
            }).ToList();

            _mockRepo.Setup(r => r.GetDogsAsync(It.IsAny<QueryParameters>())).ReturnsAsync(dogs);
            _mockMapper.Setup(m => m.Map<IEnumerable<DogDto>>(It.IsAny<IEnumerable<Dog>>())).Returns(dogsDto);

            // Act
            var result = await _service.GetDogsAsync(parameters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockRepo.Verify(r => r.GetDogsAsync(It.IsAny<QueryParameters>()), Times.Once);
        }
    }
}
