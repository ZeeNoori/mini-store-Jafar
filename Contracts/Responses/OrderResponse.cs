namespace MiniStoreManagement.Contracts.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public IEnumerable<OrderItemResponse> Items { get; set; } = [];
    }
}
