namespace CunDropShipping_Gateway.infrastructure.Clients;

public class PaymentGatewayClient : GatewayHttpClientBase, IPaymentGatewayClient
{
    public PaymentGatewayClient(HttpClient httpClient) : base(httpClient)
    {
    }
}
