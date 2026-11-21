using System.ComponentModel.DataAnnotations;

namespace MiniStoreManagement.Contracts.Requests
{
    public class OrderUpdateRequest
    {
        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        [Required, StringLength(50)]
        public string Status { get; set; } = string.Empty;
    }
}
