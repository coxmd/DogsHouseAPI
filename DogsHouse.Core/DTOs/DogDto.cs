using DogsHouse.Core.Models.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouse.Core.DTOs
{
    [ModelBinder(typeof(DogDtoModelBinder))]
    public class DogDto
    {
        /// <summary>
        /// Name of the dog. Must be unique
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Color or color pattern of the dog
        /// </summary>
        [Required(ErrorMessage = "Color is required")]
        [StringLength(30, ErrorMessage = "Color cannot be longer than 30 characters")]
        public string Color { get; set; } = string.Empty;
        /// <summary>
        /// Length of the dog's tail in centimeters. Must be non-negative
        /// </summary>
        [Range(0, 200, ErrorMessage = "Tail length must be between 0 and 200 in centimeter or meters")]
        public int TailLength { get; set; }
        /// <summary>
        /// Weight of the dog in kilograms. Must be positive
        [Range(1, 300, ErrorMessage = "Weight must be between 1 and 300 Kgs")]
        public int Weight { get; set; }
    }
}
