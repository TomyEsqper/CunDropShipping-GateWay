using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

[ApiController]
[Route("api/gateway/v1/users")]
public class UserController : ControllerBase
{
    private readonly IUserGatewayClient _userClient;

    public UserController(IUserGatewayClient userClient)
    {
        _userClient = userClient;
    }

    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _userClient.GetAsync($"/api/v1/users/{id}", cancellationToken));
    }

    [HttpPost]
    public Task<IActionResult> Create([FromBody] UserCreateDto request, CancellationToken cancellationToken)
    {
        return GatewayResultFactory.CreateAsync(
            this,
            _userClient.PostAsync("/api/v1/users", request, cancellationToken));
    }
}
