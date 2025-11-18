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
        public ActionResult<List<ProductDto>> GetAllProducts()
        {
            var domainProducts = _service.GetAllProducts();
            return Ok(_mapper.ToEntityList(domainProducts)); 
        }

        [HttpGet("{idProduct}")]
        public ActionResult<ProductDto> GetProductById(int idProduct)
        {
            var product = _service.GetProductById(idProduct);
            if (product == null) return NotFound();
            // ✅ Usamos el mapper inyectado: ToEntity
            return Ok(_mapper.ToEntity(product));
        }

        [HttpPost]
        public ActionResult<ProductDto> CreateProduct([FromBody] ProductDto productDto)
        {
            // 1. DTO -> Domain: Usamos el mapper: ToDomain
            var domainEntity = _mapper.ToDomain(productDto);
            
            // 2. Guardar
            var savedProduct = _service.SaveProduct(domainEntity);
            
            // 3. Domain -> DTO: Usamos el mapper: ToEntity
            return Ok(_mapper.ToEntity(savedProduct));
        }

        [HttpPut("{idProduct}")]
        public ActionResult<ProductDto> UpdateProduct(int idProduct, [FromBody] ProductDto productDto)
        {
            var domainEntity = _mapper.ToDomain(productDto);
            
            var updatedProduct = _service.UpdateProduct(idProduct, domainEntity);
            
            if (updatedProduct == null) return NotFound();
            
            return Ok(_mapper.ToEntity(updatedProduct));
        }

        [HttpDelete("{idProduct}")]
        public ActionResult<ProductDto> DeleteProduct(int idProduct)
        {
            var deletedProduct = _service.DeleteProduct(idProduct);
            if (deletedProduct == null) return NotFound();
            return Ok(_mapper.ToEntity(deletedProduct));
        }

        [HttpGet("search")]
        public ActionResult<List<ProductDto>> SearchProductsByName([FromQuery] string searchTerm)
        {
            var products = _service.SearchProductsByName(searchTerm);
            return Ok(_mapper.ToEntityList(products));
        }

        [HttpGet("filter/price")]
        public ActionResult<List<ProductDto>> GetProductsByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal max)
        {
            var products = _service.GetProductsByPriceRange(minPrice, max);
            return Ok(_mapper.ToEntityList(products));
        }

        [HttpGet("filter/stock")]
        public ActionResult<List<ProductDto>> GetProductsWithLowStock([FromQuery] int stockThreshold)
        {
            var products = _service.GetProductsWithLowStock(stockThreshold);
            return Ok(_mapper.ToEntityList(products));
        }
    }
}