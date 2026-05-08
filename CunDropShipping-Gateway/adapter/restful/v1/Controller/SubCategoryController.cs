using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

[ApiController]
[Route("api/gateway/v1/subcategories")]
public class SubCategoryController : ControllerBase
{
    private readonly ICatalogGatewayClient _catalogClient;

    public SubCategoryController(ICatalogGatewayClient catalogClient)
    {
        _catalogClient = catalogClient;
    }

    [HttpGet]
    public Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.GetAsync("/api/v1/subcategories", cancellationToken));
    }

    [HttpGet("{id}")]
    public Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.GetAsync($"/api/v1/subcategories/{id}", cancellationToken));
    }

    [HttpPost]
    public Task<IActionResult> Create([FromBody] CategoryDto request, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.PostAsync("/api/v1/subcategories", Request, cancellationToken));
    }

    [HttpPut("{id}")]
    public Task<IActionResult> Update(int id, [FromBody] CategoryDto request, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.PutAsync($"/api/v1/subcategories/{id}", Request, cancellationToken));
    }

    [HttpDelete("{id}")]
    public Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.DeleteAsync($"/api/v1/subcategories/{id}", cancellationToken));
    }
}
