using DogsHouse.Core.DTOs;
using DogsHouse.Core.Interfaces;
using DogsHouse.Core.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DogsHouse.API.Controllers
{
    [ApiController]
    [Route("")]
    public class DogsController : ControllerBase
    {
        private readonly IDogService _dogService;

        public DogsController(IDogService dogService)
        {
            _dogService = dogService;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Dogshouseservice.Version1.0.1");
        }

        [HttpGet("dogs")]
        public async Task<IActionResult> GetDogs([FromQuery] QueryParameters parameters)
        {
            var dogs = await _dogService.GetDogsAsync(parameters);
            return Ok(dogs);
        }

        [HttpPost("dog")]
        public async Task<IActionResult> CreateDog([FromBody] DogDto dogDto)
        {
            try
            {
                var result = await _dogService.CreateDogAsync(dogDto);
                return CreatedAtAction(nameof(GetDogs), result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
