using CunDropShipping_Gateway.infrastructure.Clients;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

[ApiController]
[Route("api/gateway/v1/carts")]
public class CartController : ControllerBase
{
    private readonly ICartGatewayClient _cartClient;

    public CartController(ICartGatewayClient cartClient)
    {
        _cartClient = cartClient;
    }

    [HttpGet("{userId:guid}")]
    public Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _cartClient.GetAsync($"/carts/buyer/{userId}", cancellationToken));
    }

    [HttpPost]
    public Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _cartClient.PostAsync("/carts", Request, cancellationToken));
    }
}
