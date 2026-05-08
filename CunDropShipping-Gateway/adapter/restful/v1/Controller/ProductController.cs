using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

[ApiController]
[Route("api/gateway/v1/products")]
public class ProductController : ControllerBase
{
    private readonly ICatalogGatewayClient _catalogClient;

    public ProductController(ICatalogGatewayClient catalogClient)
    {
        _catalogClient = catalogClient;
    }

    [HttpGet]
    public Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.GetAsync("/api/v1/products", cancellationToken));
    }

    [HttpGet("{id}")]
    public Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.GetAsync($"/api/v1/products/{id}", cancellationToken));
    }

    [HttpPost]
    public Task<IActionResult> Create([FromBody] ProductDto request, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.PostAsync("/api/v1/products", Request, cancellationToken));
    }

    [HttpPut("{id}")]
    public Task<IActionResult> Update(int id, [FromBody] ProductDto request, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.PutAsync($"/api/v1/products/{id}", Request, cancellationToken));
    }

    [HttpDelete("{id}")]
    public Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.DeleteAsync($"/api/v1/products/{id}", cancellationToken));
    }

    [HttpGet("search")]
    public Task<IActionResult> Search([FromQuery] string searchTerm, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.GetAsync($"/api/v1/products/search?searchTerm={Uri.EscapeDataString(searchTerm)}", cancellationToken));
    }

    [HttpGet("filter/price")]
    public Task<IActionResult> FilterByPrice([FromQuery] decimal min, [FromQuery] decimal max, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.GetAsync($"/api/v1/products/filter/price?min={min}&max={max}", cancellationToken));
    }

    [HttpGet("filter/stock")]
    public Task<IActionResult> GetLowStock([FromQuery] int threshold, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.GetAsync($"/api/v1/products/filter/stock?threshold={threshold}", cancellationToken));
    }
}
