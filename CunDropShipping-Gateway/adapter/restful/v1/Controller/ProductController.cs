using CunDropShipping_Gateway.infrastructure.Clients;
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

    [HttpPost]
    public Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.PostAsync("/api/v1/products", Request, cancellationToken));
    }
}
