using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.application.Common;

var builder = WebApplication.CreateBuilder(args);

// ==============================================================
// 1. CONFIGURACIÓN DE AMBIENTE Y HERRAMIENTAS
// ==============================================================

var catalogApiUrl =
    builder.Configuration.GetValue<string>("ServiceUrls:CatalogApi") ??
    builder.Configuration.GetValue<string>("ServiceUrls:ProductApi") ??
    builder.Configuration.GetValue<string>("ServiceUrls:CategoryApi") ??
    "http://localhost:5227";

var cartApiUrl =
    builder.Configuration.GetValue<string>("ServiceUrls:CartApi") ??
    "http://localhost:5153";

var userApiUrl =
    builder.Configuration.GetValue<string>("ServiceUrls:UserApi") ??
    "http://localhost:5193";

var salesApiUrl =
    builder.Configuration.GetValue<string>("ServiceUrls:SalesApi") ??
    "http://localhost:5092";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<ICatalogGatewayClient, CatalogGatewayClient>(client =>
{
    client.BaseAddress = new Uri(catalogApiUrl);
});
builder.Services.AddHttpClient<ICartGatewayClient, CartGatewayClient>(client =>
{
    client.BaseAddress = new Uri(cartApiUrl);
});
builder.Services.AddHttpClient<IUserGatewayClient, UserGatewayClient>(client =>
{
    client.BaseAddress = new Uri(userApiUrl);
});
builder.Services.AddHttpClient<IOrderGatewayClient, OrderGatewayClient>(client =>
{
    client.BaseAddress = new Uri(salesApiUrl);
});
builder.Services.AddHttpClient<IPaymentGatewayClient, PaymentGatewayClient>(client =>
{
    client.BaseAddress = new Uri(salesApiUrl);
});
builder.Services.AddScoped<GatewayValidationService>();

var app = builder.Build();

// ==============================================================
// 5. CONFIGURACIÓN DEL PIPELINE HTTP
// ==============================================================

if (app.Environment.IsDevelopment())
{
    // Habilita Swagger solo en desarrollo para documentación y pruebas
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
// Activacion Middleware de errores global
app.UseMiddleware<CunDropShipping_Gateway.application.Common.ExceptionMiddleware>();
app.MapControllers();
app.Run();

public partial class Program { }
