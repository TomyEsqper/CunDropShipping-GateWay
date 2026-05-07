using System.Net;

namespace CunDropShipping_Gateway.infrastructure.Clients;

public sealed record GatewayResponse(HttpStatusCode StatusCode, string Content, string? ContentType);
