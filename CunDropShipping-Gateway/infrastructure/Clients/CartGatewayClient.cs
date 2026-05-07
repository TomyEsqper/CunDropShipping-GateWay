namespace CunDropShipping_Gateway.infrastructure.Clients;

public sealed class CartGatewayClient : GatewayHttpClientBase, ICartGatewayClient
{
    public CartGatewayClient(HttpClient httpClient) : base(httpClient)
    {
    }
}
