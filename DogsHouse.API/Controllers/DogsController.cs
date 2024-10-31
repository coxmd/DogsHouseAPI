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

        /// <summary>
        /// Health check endpoint returning service version
        /// </summary>
        /// <returns>Service version information</returns>
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Dogshouseservice.Version1.0.1");
        }

        /// <summary>
        /// Retrieves a list of dogs based on query parameters
        /// </summary>
        /// <param name="parameters">Query parameters for filtering and pagination</param>
        /// <returns>List of dogs matching the criteria</returns>
        [HttpGet("dogs")]
        public async Task<IActionResult> GetDogs([FromQuery] QueryParameters parameters)
        {
            var dogs = await _dogService.GetDogsAsync(parameters);
            return Ok(dogs);
        }

        /// <summary>
        /// Creates a new dog
        /// </summary>
        /// <param name="dogDto">Dog creation data</param>
        /// <returns>Created dog information</returns>
        /// <response code="201">Returns the newly created dog</response>
        /// <response code="400">If the dog data is invalid</response>
        [HttpPost("dog")]
        public async Task<IActionResult> CreateDog([FromBody] DogDto dogDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { errors });
            }

            try
            {
                var result = await _dogService.CreateDogAsync(dogDto);
                return CreatedAtAction(nameof(GetDogs), result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
