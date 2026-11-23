using System;
using System.ComponentModel.DataAnnotations;

namespace MiniStoreManagement.Contracts.Requests
{
    public class CustomerRequest
    {
        [Required, MinLength(50)]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = "";

    }
}
