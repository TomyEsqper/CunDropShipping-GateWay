using CunDropShipping_Gateway.infrastructure.Clients;
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

    [HttpPost]
    public Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _catalogClient.PostAsync("/api/v1/subcategories", Request, cancellationToken));
    }
}
