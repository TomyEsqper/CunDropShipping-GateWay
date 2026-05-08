namespace CunDropShipping_Gateway.infrastructure.Clients;

public interface IPaymentGatewayClient
{
    Task<GatewayResponse> GetAsync(string path, CancellationToken cancellationToken);
    Task<GatewayResponse> PostAsync<T>(string path, T body, CancellationToken cancellationToken);
    Task<GatewayResponse> PostAsync(string path, HttpRequest request, CancellationToken cancellationToken);
}
