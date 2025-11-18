using CunDropShipping_Gateway.application.Common;
using CunDropShipping_Gateway.application.Service;
using CunDropShipping_Gateway.domain;
using CunDropShipping_Gateway.domain.Entity;
using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.infrastructure.Entity;
using CunDropShipping_Gateway.infrastructure.Mapper;
// ¡IMPORTANTE! Agrega este using para que encuentre CategoryDto
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity; 
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Mapper; // Para el CategoryAdapterMapper

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CONFIGURACIÓN DE URLS
// ==========================================
var productApiUrl = builder.Configuration.GetValue<string>("ServiceUrls:ProductApi");
var categoryApiUrl = builder.Configuration.GetValue<string>("ServiceUrls:CategoryApi"); 

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================================
// 2. CLIENTES HTTP (INFRASTRUCTURE)
// ==========================================
builder.Services.AddHttpClient<IProductClient, ProductClient>(client =>
{
    client.BaseAddress = new Uri(productApiUrl);
});

builder.Services.AddHttpClient<ICategoryClient, CategoryClient>(client =>
{
    client.BaseAddress = new Uri(categoryApiUrl);
});

// ==========================================
// 3. SERVICIOS (DOMAIN/APPLICATION)
// ==========================================
builder.Services.AddScoped<IGatewayService, GatewayServiceImp>(); 
builder.Services.AddScoped<ICategoryService, CategoryServiceImp>();

// ==========================================
// 4. MAPPERS (TRADUCTORES)
// ==========================================

// A. Mapper Infraestructura (Domain <-> Infrastructure Response)
builder.Services.AddScoped<IMapper<DomainCategoryEntity, CategoryResponse>, CategoryInfrastructureMapper>();

// B. Mapper Adapter (Domain <-> Adapter Dto)
builder.Services.AddScoped<IMapper<DomainCategoryEntity, CategoryDto>, CategoryAdapterMapper>(); 


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