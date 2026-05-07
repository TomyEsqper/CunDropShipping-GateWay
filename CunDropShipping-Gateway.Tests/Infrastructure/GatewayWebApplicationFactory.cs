using CunDropShipping_Gateway.infrastructure.Clients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace CunDropShipping_Gateway.Tests.Infrastructure;

public sealed class GatewayWebApplicationFactory : WebApplicationFactory<Program>
{
    public StubHttpMessageHandler CatalogHandler { get; set; } =
        new(_ => StubHttpMessageHandler.Json(System.Net.HttpStatusCode.OK, "[]"));

    public StubHttpMessageHandler CartHandler { get; set; } =
        new(_ => StubHttpMessageHandler.Json(System.Net.HttpStatusCode.OK, "{}"));

    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.AddHttpClient<ICatalogGatewayClient, CatalogGatewayClient>()
                .ConfigurePrimaryHttpMessageHandler(() => CatalogHandler)
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://catalog.test"));

            services.AddHttpClient<ICartGatewayClient, CartGatewayClient>()
                .ConfigurePrimaryHttpMessageHandler(() => CartHandler)
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://cart.test"));
        });
    }
}
