using System.ComponentModel.DataAnnotations;

namespace MiniStoreManagement.Contracts.Requests
{
    public class ProductRequest
    {
        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Range(typeof(decimal), "0", "9999999999999999.99")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
    }
}
