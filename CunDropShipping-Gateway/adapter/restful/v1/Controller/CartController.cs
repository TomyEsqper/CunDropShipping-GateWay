using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;
using CunDropShipping_Gateway.application.Common;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

[ApiController]
[Route("api/gateway/v1/carts")]
public class CartController : ControllerBase
{
    private readonly ICartGatewayClient _cartClient;
    private readonly GatewayValidationService _validator;

    public CartController(ICartGatewayClient cartClient, GatewayValidationService validator)
    {
        _cartClient = cartClient;
        _validator = validator;
    }

    [HttpGet("{userId:guid}")]
    public Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _cartClient.GetAsync($"/carts/buyer/{userId}", cancellationToken));
    }

    [HttpPost]
    public Task<IActionResult> Create([FromBody] CreateShoppingCartRequest request, CancellationToken cancellationToken)
    {
        return CreateValidatedAsync(request, cancellationToken);
    }

    private async Task<IActionResult> CreateValidatedAsync(CreateShoppingCartRequest request, CancellationToken cancellationToken)
    {
        await _validator.EnsureUserExistsAsync(request.BuyerId, cancellationToken);
        return await GatewayResultFactory.CreateAsync(this, _cartClient.PostAsync("/carts", request, cancellationToken));
    }

    [HttpDelete("{cartId:guid}")]
    public Task<IActionResult> Delete(Guid cartId, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _cartClient.DeleteAsync($"/carts/{cartId}", cancellationToken));
    }
}
