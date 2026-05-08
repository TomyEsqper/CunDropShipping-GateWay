namespace CunDropShipping_Gateway.infrastructure.Clients;

public class OrderGatewayClient : GatewayHttpClientBase, IOrderGatewayClient
{
    public OrderGatewayClient(HttpClient httpClient) : base(httpClient)
    {
    }
}
