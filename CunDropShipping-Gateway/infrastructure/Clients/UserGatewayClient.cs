namespace CunDropShipping_Gateway.infrastructure.Clients;

public sealed class UserGatewayClient : GatewayHttpClientBase, IUserGatewayClient
{
    public UserGatewayClient(HttpClient httpClient) : base(httpClient)
    {
    }
}
