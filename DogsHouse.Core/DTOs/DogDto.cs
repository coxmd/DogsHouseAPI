using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouse.Core.DTOs
{
    public class DogDto
    {
        /// <summary>
        /// Name of the dog. Must be unique
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Color or color pattern of the dog
        /// </summary>
        public string Color { get; set; } = string.Empty;
        /// <summary>
        /// Length of the dog's tail in centimeters. Must be non-negative
        /// </summary>
        public int TailLength { get; set; }
        /// <summary>
        /// Weight of the dog in kilograms. Must be positive
        public int Weight { get; set; }
    }
}
