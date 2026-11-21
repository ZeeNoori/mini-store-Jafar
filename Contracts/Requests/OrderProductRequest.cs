using System.ComponentModel.DataAnnotations;

namespace MiniStoreManagement.Contracts.Requests
{
    public class OrderProductRequest
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
