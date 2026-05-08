using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;
using CunDropShipping_Gateway.application.Common;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

[ApiController]
[Route("api/gateway/v1/payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentGatewayClient _paymentClient;
    private readonly GatewayValidationService _validator;

    public PaymentController(IPaymentGatewayClient paymentClient, GatewayValidationService validator)
    {
        _paymentClient = paymentClient;
        _validator = validator;
    }

    [HttpPost("process")]
    public Task<IActionResult> ProcessPayment([FromBody] AdapterPaymentEntity request, CancellationToken cancellationToken)
    {
        return ProcessValidatedAsync(request, cancellationToken);
    }

    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _paymentClient.GetAsync($"/api/v1/payments/{id}", cancellationToken));
    }

    private async Task<IActionResult> ProcessValidatedAsync(AdapterPaymentEntity request, CancellationToken cancellationToken)
    {
        await _validator.EnsureOrderExistsAsync(request.OrderId, cancellationToken);
        return await GatewayResultFactory.CreateAsync(this, _paymentClient.PostAsync("/api/v1/payments/process", request, cancellationToken));
    }
}
