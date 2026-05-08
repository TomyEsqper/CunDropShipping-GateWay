using CunDropShipping_Gateway.infrastructure.Clients;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

[ApiController]
[Route("api/gateway/v1/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderGatewayClient _orderClient;

    public OrderController(IOrderGatewayClient orderClient)
    {
        _orderClient = orderClient;
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
    public Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _orderClient.PostAsync("/api/v1/orders", Request, cancellationToken));
    }
}
