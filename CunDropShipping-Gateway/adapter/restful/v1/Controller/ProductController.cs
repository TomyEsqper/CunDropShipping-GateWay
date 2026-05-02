using CunDropShipping_Gateway.application.Service;
using CunDropShipping_Gateway.domain.Entity;
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;
using Microsoft.AspNetCore.Mvc;
using CunDropShipping_Gateway.application.Common; 

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller
{
    [ApiController]
    [Route("api/gateway/v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper<DomainProductEntity, ProductDto> _mapper; 

        // ✅ NUEVO: El constructor ahora recibe el Mapper
        public ProductController(IProductService service, IMapper<DomainProductEntity, ProductDto> mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        
        [HttpGet]
        // [EDUCATIVO] Cambiamos a 'async Task<ActionResult<...>>'.
        // Esto le dice al servidor web (Kestrel): "Libera el hilo mientras proceso esto".
        public async Task<ActionResult<List<ProductDto>>> GetAllProducts()
        {
            // [EDUCATIVO] 'await' aquí es crucial. Si usaras .Result aquí, 
            // anularías todo el trabajo previo y volverías a bloquear.
            var domainProducts = await _service.GetAllProducts();
            return Ok(_mapper.ToEntityList(domainProducts)); 
        }

        [HttpGet("{idProduct}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int idProduct)
        {
            var product = await _service.GetProductById(idProduct);
            
            if (product == null) return NotFound();
            
            return Ok(_mapper.ToEntity(product));
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductDto productDto)
        {
            var domainEntity = _mapper.ToDomain(productDto);
            
            var savedProduct = await _service.SaveProduct(domainEntity);
            
            return Ok(_mapper.ToEntity(savedProduct));
        }

        [HttpPut("{idProduct}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int idProduct, [FromBody] ProductDto productDto)
        {
            var domainEntity = _mapper.ToDomain(productDto);
            
            var updatedProduct = await _service.UpdateProduct(idProduct, domainEntity);
            
            if (updatedProduct == null) return NotFound();
            
            return Ok(_mapper.ToEntity(updatedProduct));
        }

        [HttpDelete("{idProduct}")]
        public async Task<ActionResult<ProductDto>> DeleteProduct(int idProduct)
        {
            var deletedProduct = await _service.DeleteProduct(idProduct);
            if (deletedProduct == null) return NotFound();
            return Ok(_mapper.ToEntity(deletedProduct));
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ProductDto>>> SearchProductsByName([FromQuery] string searchTerm)
        {
            var products = await _service.SearchProductsByName(searchTerm);
            return Ok(_mapper.ToEntityList(products));
        }

        [HttpGet("filter/price")]
        public async Task<ActionResult<List<ProductDto>>> GetProductsByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal max)
        {
            var products = await _service.GetProductsByPriceRange(minPrice, max);
            return Ok(_mapper.ToEntityList(products));
        }

        [HttpGet("filter/stock")]
        public async Task<ActionResult<List<ProductDto>>> GetProductsWithLowStock([FromQuery] int stockThreshold)
        {
            var products = await _service.GetProductsWithLowStock(stockThreshold);
            return Ok(_mapper.ToEntityList(products));
        }
    }
}
