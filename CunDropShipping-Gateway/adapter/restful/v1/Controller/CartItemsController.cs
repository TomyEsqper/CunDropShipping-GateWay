using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

[ApiController]
[Route("api/gateway/v1/cart-items")]
public class CartItemsController : ControllerBase
{
    private readonly ICartGatewayClient _cartClient;

    public CartItemsController(ICartGatewayClient cartClient)
    {
        _cartClient = cartClient;
    }

    [HttpPost]
    public Task<IActionResult> AddOrUpdateItem([FromBody] UpsertCartItemRequest request, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _cartClient.PostAsync("/cart-items", request, cancellationToken));
    }

    [HttpPatch("{cartId:guid}/{productId}")]
    public Task<IActionResult> UpdateItemQuantity(Guid cartId, int productId, [FromBody] UpdateCartItemQuantityRequest request, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _cartClient.PatchAsync($"/cart-items/{cartId}/{productId}", request, cancellationToken));
    }

    [HttpDelete("{cartId:guid}/{productId}")]
    public Task<IActionResult> RemoveItem(Guid cartId, int productId, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _cartClient.DeleteAsync($"/cart-items/{cartId}/{productId}", cancellationToken));
    }
}
