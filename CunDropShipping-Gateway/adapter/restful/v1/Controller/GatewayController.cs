using CunDropShipping_Gateway.application.Service;
using CunDropShipping_Gateway.domain.Entity; // Tu entidad de negocio (Domain)
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity; // Aquí solo debe estar ProductDto
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller
{
    [ApiController]
    [Route("api/gateway/v1/products")]
    public class GatewayController : ControllerBase
    {
        private readonly IGatewayService _gatewayService;

        public GatewayController(IGatewayService gatewayService)
        {
            _gatewayService = gatewayService;
        }

        // ==============================================================
        // 🛠️ MAPPERS (Traductor interno del Controller)
        // ==============================================================
        
        // 1. De Dominio -> DTO (Para responder al cliente)
        private ProductDto ToDto(Product domain)
        {
            if (domain == null) return null;
            return new ProductDto
            {
                IdProduct = domain.IdProduct,
                NameProduct = domain.NameProduct,
                Description = domain.Description,
                Price = domain.Price,
                StockQuantity = domain.StockQuantity
            };
        }

        // 2. De DTO -> Dominio (Para enviar al Servicio)
        private Product ToDomain(ProductDto dto)
        {
            return new Product
            {
                IdProduct = dto.IdProduct,
                NameProduct = dto.NameProduct,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity
            };
        }

        // ==============================================================
        // 🌐 ENDPOINTS (Todos usan ProductDto)
        // ==============================================================

        [HttpGet]
        public ActionResult<List<ProductDto>> GetAllProducts()
        {
            var products = _gatewayService.GetAllProducts();
            // Convertimos la lista de Dominio a DTOs
            return Ok(products.Select(p => ToDto(p)).ToList());
        }

        [HttpGet("{idProduct}")]
        public ActionResult<ProductDto> GetProductById(int idProduct)
        {
            var product = _gatewayService.GetProductById(idProduct);
            if (product == null) return NotFound();
            return Ok(ToDto(product));
        }

        [HttpPost]
        public ActionResult<ProductDto> SaveProduct([FromBody] ProductDto productDto)
        {
            // Convertimos el DTO a Dominio para pasarlo al servicio
            var domainEntity = ToDomain(productDto);
            
            // El servicio devuelve la entidad creada (Dominio)
            var createdProduct = _gatewayService.SaveProduct(domainEntity);
            
            // Convertimos de vuelta a DTO para responder
            var responseDto = ToDto(createdProduct);
            
            return CreatedAtAction(nameof(GetProductById), new { idProduct = responseDto.IdProduct }, responseDto);
        }

        [HttpPut("{idProduct}")]
        public ActionResult<ProductDto> UpdateProduct(int idProduct, [FromBody] ProductDto productDto)
        {
            var domainEntity = ToDomain(productDto);
            var updatedProduct = _gatewayService.UpdateProduct(idProduct, domainEntity);
            
            if (updatedProduct == null) return NotFound();
            return Ok(ToDto(updatedProduct));
        }

        [HttpDelete("{idProduct}")]
        public ActionResult<ProductDto> DeleteProduct(int idProduct)
        {
            var deletedProduct = _gatewayService.DeleteProduct(idProduct);
            if (deletedProduct == null) return NotFound();
            return Ok(ToDto(deletedProduct));
        }

        [HttpGet("search")]
        public ActionResult<List<ProductDto>> SearchProductsByName([FromQuery] string searchTerm)
        {
            var products = _gatewayService.SearchProductsByName(searchTerm);
            return Ok(products.Select(p => ToDto(p)).ToList());
        }

        [HttpGet("filter/price")]
        public ActionResult<List<ProductDto>> GetProductsByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal max)
        {
            var products = _gatewayService.GetProductsByPriceRange(minPrice, max);
            return Ok(products.Select(p => ToDto(p)).ToList());
        }

        [HttpGet("filter/stock")]
        public ActionResult<List<ProductDto>> GetProductsWithLowStock([FromQuery] int stockThreshold)
        {
            var products = _gatewayService.GetProductsWithLowStock(stockThreshold);
            return Ok(products.Select(p => ToDto(p)).ToList());
        }
    }
}