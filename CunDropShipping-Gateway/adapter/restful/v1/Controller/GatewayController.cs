using CunDropShipping_Gateway.application.Service;
using CunDropShipping_Gateway.infrastructure.Entity; 
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller
{
    [ApiController]
    [Route("api/gateway/v1/products")] // <-- Ruta en plural (es estándar)
    public class GatewayController : ControllerBase
    {
        private readonly IGatewayService _gatewayService;

        public GatewayController(IGatewayService gatewayService)
        {
            _gatewayService = gatewayService;
        }

        [HttpGet]
        public ActionResult<List<ProductResponse>> GetAllProducts()
        {
            // OJO: Esto asume que tu IGatewayService devuelve un 'ProductResponse' del adapter
            // (Lo ideal es que devuelva un 'DomainEntity' y tú lo mapees aquí)
            var products = _gatewayService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{idProduct}")] // <-- El nombre del parámetro DEBE coincidir
        public ActionResult<ProductResponse> GetProductById(int idProduct)
        {
            var product = _gatewayService.GetProductById(idProduct);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public ActionResult<ProductResponse> SaveProduct([FromBody] ProductRequest request)
        {
            // Tienes que crear este método 'SaveProduct' en tu IGatewayService
            var product = _gatewayService.SaveProduct(request); 
            return CreatedAtAction(nameof(GetProductById), new { idProduct = product.IdProduct }, product);
        }

        [HttpPut("{idProduct}")] // <-- El nombre del parámetro DEBE coincidir
        public ActionResult<ProductResponse> UpdateProduct(int idProduct, [FromBody] ProductRequest request)
        {
            // Tienes que crear este método 'UpdateProduct' en tu IGatewayService
            var product = _gatewayService.UpdateProduct(idProduct, request);
            return Ok(product);
        }

        [HttpDelete("{idProduct}")] // <-- El nombre del parámetro DEBE coincidir
        public ActionResult<ProductResponse> DeleteProduct(int idProduct)
        {
            // Tienes que crear este método 'DeleteProduct' en tu IGatewayService
            var product = _gatewayService.DeleteProduct(idProduct);
            return Ok(product);
        }

        [HttpGet("search")] // <-- Ruta más limpia
        public ActionResult<List<ProductResponse>> SearchProductsByName([FromQuery] string searchTerm)
        {
            var products = _gatewayService.SearchProductsByName(searchTerm);
            return Ok(products);
        }

        [HttpGet("filter/price")]
        public ActionResult<List<ProductResponse>> GetProductsByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal max)
        {
            var products = _gatewayService.GetProductsByPriceRange(minPrice, max);
            return Ok(products);
        }

        [HttpGet("filter/stock")]
        public ActionResult<List<ProductResponse>> GetProductsWithLowStock([FromQuery] int stockThreshold)
        {
            var products = _gatewayService.GetProductsWithLowStock(stockThreshold);
            return Ok(products);
        }
    }
}