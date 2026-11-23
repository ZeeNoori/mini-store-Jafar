using System;
using System.ComponentModel.DataAnnotations;

namespace MiniStoreManagement.Contracts.Requests
{
    public class CategoryRequest
    {
        [Required, MinLength(5)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(500, MinimumLength = 5)]
        public string Description { get; set; } = string.Empty;
    }
}
