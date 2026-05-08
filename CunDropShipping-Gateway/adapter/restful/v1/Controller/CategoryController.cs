using CunDropShipping_Gateway.infrastructure.Clients;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

[ApiController]
[Route("api/gateway/v1/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICatalogGatewayClient _catalogClient;

    public CategoryController(ICatalogGatewayClient catalogClient)
    {
        _catalogClient = catalogClient;
    }

    [HttpGet]
    public Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.GetAsync("/api/v1/categories", cancellationToken));
    }

    [HttpGet("{id}")]
    public Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.GetAsync($"/api/v1/categories/{id}", cancellationToken));
    }

    [HttpPost]
    public Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.PostAsync("/api/v1/categories", Request, cancellationToken));
    }

    [HttpPut("{id}")]
    public Task<IActionResult> Update(int id, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.PutAsync($"/api/v1/categories/{id}", Request, cancellationToken));
    }

    [HttpDelete("{id}")]
    public Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.DeleteAsync($"/api/v1/categories/{id}", cancellationToken));
    }
}
