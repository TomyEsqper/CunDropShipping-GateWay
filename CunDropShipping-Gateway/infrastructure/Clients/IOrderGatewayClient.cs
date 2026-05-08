namespace CunDropShipping_Gateway.infrastructure.Clients;

public interface IOrderGatewayClient
{
    Task<GatewayResponse> GetAsync(string path, CancellationToken cancellationToken);
    Task<GatewayResponse> PostAsync(string path, HttpRequest request, CancellationToken cancellationToken);
}
