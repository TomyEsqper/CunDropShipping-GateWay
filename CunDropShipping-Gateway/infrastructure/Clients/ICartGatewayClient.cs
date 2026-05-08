namespace CunDropShipping_Gateway.infrastructure.Clients;

public interface ICartGatewayClient
{
    Task<GatewayResponse> GetAsync(string path, CancellationToken cancellationToken);
    Task<GatewayResponse> PostAsync<T>(string path, T body, CancellationToken cancellationToken);
    Task<GatewayResponse> PostAsync(string path, HttpRequest request, CancellationToken cancellationToken);
    Task<GatewayResponse> DeleteAsync(string path, CancellationToken cancellationToken);
    Task<GatewayResponse> PatchAsync<T>(string path, T body, CancellationToken cancellationToken);
    Task<GatewayResponse> PatchAsync(string path, HttpRequest request, CancellationToken cancellationToken);
}
