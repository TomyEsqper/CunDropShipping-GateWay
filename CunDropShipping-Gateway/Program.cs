using CunDropShipping_Gateway.application.Service;
using CunDropShipping_Gateway.domain;
using CunDropShipping_Gateway.infrastructure.Clients;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Lee la URL del appsettings ---
var productApiUrl = builder.Configuration
    .GetValue<string>("ServiceUrls:ProductApi");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- 2. Registra el HttpClient para el ProductClient ---
builder.Services.AddHttpClient<IProductClient, ProductClient>(client =>
{
    client.BaseAddress = new Uri(productApiUrl);
});

// --- 3. Aquí conectamos la interfaz con su implementación (el "Cerebro")
builder.Services.AddScoped<IGatewayService, GatewayServiceImp>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();