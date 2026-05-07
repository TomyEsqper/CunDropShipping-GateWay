namespace CunDropShipping_Gateway.infrastructure.Clients;

public sealed class CatalogGatewayClient : GatewayHttpClientBase, ICatalogGatewayClient
{
    public CatalogGatewayClient(HttpClient httpClient) : base(httpClient)
    {
    }
}
