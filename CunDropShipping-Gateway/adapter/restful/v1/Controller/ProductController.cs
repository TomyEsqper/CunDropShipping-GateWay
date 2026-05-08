using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;
using CunDropShipping_Gateway.application.Common;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

[ApiController]
[Route("api/gateway/v1/products")]
public class ProductController : ControllerBase
{
    private readonly ICatalogGatewayClient _catalogClient;
    private readonly GatewayValidationService _validator;

    public ProductController(ICatalogGatewayClient catalogClient, GatewayValidationService validator)
    {
        _catalogClient = catalogClient;
        _validator = validator;
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
        return CreateValidatedAsync(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public Task<IActionResult> Update(int id, [FromBody] ProductDto request, CancellationToken cancellationToken)
    {
        return UpdateValidatedAsync(id, request, cancellationToken);
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

    private async Task<IActionResult> CreateValidatedAsync(ProductDto request, CancellationToken cancellationToken)
    {
        await _validator.EnsureCategoryExistsAsync(request.IdCategory, cancellationToken);
        return await GatewayResultFactory.CreateAsync(this, _catalogClient.PostAsync("/api/v1/products", request, cancellationToken));
    }

    private async Task<IActionResult> UpdateValidatedAsync(int id, ProductDto request, CancellationToken cancellationToken)
    {
        await _validator.EnsureCategoryExistsAsync(request.IdCategory, cancellationToken);
        return await GatewayResultFactory.CreateAsync(this, _catalogClient.PutAsync($"/api/v1/products/{id}", request, cancellationToken));
    }
}
