using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;
using CunDropShipping_Gateway.application.Common;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

[ApiController]
[Route("api/gateway/v1/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderGatewayClient _orderClient;
    private readonly GatewayValidationService _validator;

    public OrderController(IOrderGatewayClient orderClient, GatewayValidationService validator)
    {
        _orderClient = orderClient;
        _validator = validator;
    }

    [HttpGet]
    public Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _orderClient.GetAsync("/api/v1/orders", cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _orderClient.GetAsync($"/api/v1/orders/{id}", cancellationToken));
    }

    [HttpPost]
    public Task<IActionResult> Create([FromBody] AdapterOrderEntity request, CancellationToken cancellationToken)
    {
        return CreateValidatedAsync(request, cancellationToken);
    }

    private async Task<IActionResult> CreateValidatedAsync(AdapterOrderEntity request, CancellationToken cancellationToken)
    {
        await _validator.EnsureUserExistsAsync(request.BuyerId, cancellationToken);
        await _validator.EnsureCartForUserExistsAsync(request.BuyerId, cancellationToken);
        await _validator.EnsureCartHasItemsAsync(request.BuyerId, cancellationToken);
        return await GatewayResultFactory.CreateAsync(this, _orderClient.PostAsync("/api/v1/orders", request, cancellationToken));
    }
}
