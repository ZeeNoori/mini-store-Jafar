using System;

namespace MiniStoreManagement.Contracts.Requests
{
    public class CreateCustomerRequest
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = "";

    }
}
