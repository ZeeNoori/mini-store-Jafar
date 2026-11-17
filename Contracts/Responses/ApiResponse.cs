using System;

namespace MiniStoreManagement.Contracts.Responses
{
    public class ApiResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
    }
}
