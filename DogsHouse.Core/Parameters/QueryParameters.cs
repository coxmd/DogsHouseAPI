using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouse.Core.Parameters
{
    /// <summary>
    /// Parameters for querying and filtering dog data
    /// </summary>
    public class QueryParameters
    {
        // Maximum allowed page size to prevent excessive data retrieval
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        /// <summary>
        /// Current page number (1-based). Defaults to 1
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Number of items per page. Maximum value is 50
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        /// <summary>
        /// Attribute to sort by (name, weight, or tail_length)
        /// </summary>
        public string? Attribute { get; set; }

        /// <summary>
        /// Sort order (asc or desc)
        /// </summary>
        public string? Order { get; set; }
    }
}
