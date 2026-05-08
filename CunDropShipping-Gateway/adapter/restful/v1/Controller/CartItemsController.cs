using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;
using CunDropShipping_Gateway.application.Common;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

[ApiController]
[Route("api/gateway/v1/cart-items")]
public class CartItemsController : ControllerBase
{
    private readonly ICartGatewayClient _cartClient;
    private readonly GatewayValidationService _validator;

    public CartItemsController(ICartGatewayClient cartClient, GatewayValidationService validator)
    {
        _cartClient = cartClient;
        _validator = validator;
    }

    [HttpPost]
    public Task<IActionResult> AddOrUpdateItem([FromBody] UpsertCartItemRequest request, CancellationToken cancellationToken)
    {
        return AddValidatedAsync(request, cancellationToken);
    }

    [HttpPatch("{cartId:guid}/{productId}")]
    public Task<IActionResult> UpdateItemQuantity(Guid cartId, int productId, [FromBody] UpdateCartItemQuantityRequest request, CancellationToken cancellationToken)
    {
        return UpdateValidatedAsync(cartId, productId, request, cancellationToken);
    }

    [HttpDelete("{cartId:guid}/{productId}")]
    public Task<IActionResult> RemoveItem(Guid cartId, int productId, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _cartClient.DeleteAsync($"/cart-items/{cartId}/{productId}", cancellationToken));
    }

    private async Task<IActionResult> AddValidatedAsync(UpsertCartItemRequest request, CancellationToken cancellationToken)
    {
        await _validator.EnsureProductExistsAsync(request.ProductId, cancellationToken);
        return await GatewayResultFactory.CreateAsync(this, _cartClient.PostAsync("/cart-items", request, cancellationToken));
    }

    private async Task<IActionResult> UpdateValidatedAsync(Guid cartId, int productId, UpdateCartItemQuantityRequest request, CancellationToken cancellationToken)
    {
        await _validator.EnsureProductExistsAsync(productId, cancellationToken);
        return await GatewayResultFactory.CreateAsync(this, _cartClient.PatchAsync($"/cart-items/{cartId}/{productId}", request, cancellationToken));
    }
}
