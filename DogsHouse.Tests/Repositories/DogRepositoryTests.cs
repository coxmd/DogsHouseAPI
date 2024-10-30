using DogsHouse.Core.Models;
using DogsHouse.Core.Parameters;
using DogsHouse.Infrastructure.Data;
using DogsHouse.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DogsHouse.Tests.Repositories
{
    public class DogRepositoryTests
    {
        private readonly DbContextOptions<DogsContext> _options;
        private readonly DogsContext _context;
        private readonly DogRepository _repository;

        public DogRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DogsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DogsContext(_options);
            _repository = new DogRepository(_context);
        }

        [Fact]
        public async Task AddDogAsync_AddsNewDog_Successfully()
        {
            // Arrange
            var dog = new Dog { Name = "TestDog", Color = "Brown", TailLength = 10, Weight = 20 };

            // Act
            var result = await _repository.AddDogAsync(dog);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dog.Name, result.Name);
            Assert.True(result.Id > 0);
            var savedDog = await _context.Dogs.FindAsync(result.Id);
            Assert.NotNull(savedDog);
        }

        [Fact]
        public async Task GetDogByNameAsync_ReturnsCorrectDog()
        {
            // Arrange
            var dog = new Dog { Name = "TestDog", Color = "Brown", TailLength = 10, Weight = 20 };
            _context.Dogs.Add(dog);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetDogByNameAsync("TestDog");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dog.Name, result.Name);
            Assert.Equal(dog.Color, result.Color);
        }

        [Fact]
        public async Task GetDogsAsync_WithPagination_ReturnsCorrectDogs()
        {
            // Arrange
            var dogs = new List<Dog>
            {
                new() { Name = "Dog1", Color = "Brown", TailLength = 10, Weight = 20 },
                new() { Name = "Dog2", Color = "Black", TailLength = 15, Weight = 25 },
                new() { Name = "Dog3", Color = "White", TailLength = 12, Weight = 22 }
            };
            _context.Dogs.AddRange(dogs);
            await _context.SaveChangesAsync();

            var parameters = new QueryParameters { PageNumber = 1, PageSize = 2 };

            // Act
            var result = await _repository.GetDogsAsync(parameters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Theory]
        [InlineData("name", "asc")]
        [InlineData("name", "desc")]
        [InlineData("weight", "asc")]
        [InlineData("weight", "desc")]
        [InlineData("tail_length", "asc")]
        [InlineData("tail_length", "desc")]
        public async Task GetDogsAsync_WithSorting_ReturnsSortedDogs(string attribute, string order)
        {
            // Arrange
            var dogs = new List<Dog>
            {
                new() { Name = "Charlie", Color = "Brown", TailLength = 10, Weight = 20 },
                new() { Name = "Alpha", Color = "Black", TailLength = 15, Weight = 25 },
                new() { Name = "Bravo", Color = "White", TailLength = 12, Weight = 22 }
            };
            _context.Dogs.AddRange(dogs);
            await _context.SaveChangesAsync();

            var parameters = new QueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                Attribute = attribute,
                Order = order
            };

            // Act
            var result = await _repository.GetDogsAsync(parameters);
            var resultList = result.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, resultList.Count);

            if (attribute == "name" && order == "asc")
                Assert.Equal("Alpha", resultList.First().Name);
            else if (attribute == "name" && order == "desc")
                Assert.Equal("Charlie", resultList.First().Name);
            else if (attribute == "weight" && order == "asc")
                Assert.Equal(20, resultList.First().Weight);
            else if (attribute == "weight" && order == "desc")
                Assert.Equal(25, resultList.First().Weight);
            else if (attribute == "tail_length" && order == "asc")
                Assert.Equal(10, resultList.First().TailLength);
            else if (attribute == "tail_length" && order == "desc")
                Assert.Equal(15, resultList.First().TailLength);
        }

        [Fact]
        public async Task GetTotalCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            var dogs = new List<Dog>
            {
                new() { Name = "Dog1", Color = "Brown", TailLength = 10, Weight = 20 },
                new() { Name = "Dog2", Color = "Black", TailLength = 15, Weight = 25 }
            };
            _context.Dogs.AddRange(dogs);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetTotalCountAsync();

            // Assert
            Assert.Equal(2, result);
        }
    }
}
