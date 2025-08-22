namespace backend.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsHotSeller { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
