using backend.DTOs;
using backend.Models;
using backend.Services.Poroduct;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _env;

        public ProductsController(IProductService productService, IWebHostEnvironment env)
        {
            _productService = productService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategory(categoryId);
            return Ok(products);
        }

        [HttpGet("hotsellers")]
        public async Task<IActionResult> GetHotSellers()
        {
            var products = await _productService.GetHotSellers();
            return Ok(products);
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest([FromQuery] int count = 10)
        {
            var products = await _productService.GetLatestProducts(count);
            return Ok(products);
        }

        // Create product + image upload
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto dto)
        {
            if (dto == null) return BadRequest("Invalid data.");

            // Save image if provided
            string? imageUrl = null;
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploads = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "products");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var ext = Path.GetExtension(dto.Image.FileName);
                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                imageUrl = $"/uploads/products/{fileName}";
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = imageUrl,
                IsHotSeller = dto.IsHotSeller,
                CreatedDate = DateTime.UtcNow,
                CategoryId = dto.CategoryId
            };

            var newId = await _productService.AddProduct(product);
            return CreatedAtAction(nameof(GetById), new { id = newId }, new { ProductId = newId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductCreateDto dto)
        {
            // For simplicity reuse ProductCreateDto for updates (image optional).
            // Ensure id validity in real app.
            var existing = await _productService.GetProductById(id);
            if (existing == null) return NotFound();

            string? imageUrl = existing.ImageUrl;

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploads = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "products");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var ext = Path.GetExtension(dto.Image.FileName);
                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                imageUrl = $"/uploads/products/{fileName}";
            }

            var product = new Product
            {
                ProductId = id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = imageUrl,
                IsHotSeller = dto.IsHotSeller,
                CategoryId = dto.CategoryId
            };

            await _productService.UpdateProduct(product);
            return Ok(new { Message = "Product updated" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProduct(id);
            return Ok(new { Message = "Product deleted" });
        }



    }
}
