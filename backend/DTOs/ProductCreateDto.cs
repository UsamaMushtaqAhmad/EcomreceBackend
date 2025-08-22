using Microsoft.AspNetCore.Http;



namespace backend.DTOs


{
    public class ProductCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public bool IsHotSeller { get; set; } = false;
        public IFormFile? Image { get; set; } // bind from multipart/form-data
    }
}
