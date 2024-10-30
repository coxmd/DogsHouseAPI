using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouse.Core.Models
{
    /// <summary>
    /// Represents a dog entity with its basic characteristics
    /// </summary>
    public class Dog
    {
        /// <summary>
        /// Unique identifier for the dog
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the dog. Must be unique in the system
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Color or color pattern of the dog
        /// </summary>
        public string Color { get; set; } = string.Empty;
        /// <summary>
        /// Length of the dog's tail in centimeters
        /// </summary>
        public int TailLength { get; set; }
        /// <summary>
        /// Weight of the dog in kilograms
        /// </summary>
        public int Weight { get; set; }
    }
}
