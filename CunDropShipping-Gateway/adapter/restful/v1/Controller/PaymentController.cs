using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

[ApiController]
[Route("api/gateway/v1/payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentGatewayClient _paymentClient;

    public PaymentController(IPaymentGatewayClient paymentClient)
    {
        _paymentClient = paymentClient;
    }

    [HttpPost("process")]
    public Task<IActionResult> ProcessPayment([FromBody] AdapterPaymentEntity request, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _paymentClient.PostAsync("/api/v1/payments/process", request, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _paymentClient.GetAsync($"/api/v1/payments/{id}", cancellationToken));
    }
}
