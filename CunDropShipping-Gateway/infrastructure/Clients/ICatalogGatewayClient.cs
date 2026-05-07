namespace CunDropShipping_Gateway.infrastructure.Clients;

public interface ICatalogGatewayClient
{
    Task<GatewayResponse> GetAsync(string path, CancellationToken cancellationToken);
    Task<GatewayResponse> PostAsync(string path, HttpRequest request, CancellationToken cancellationToken);
}
