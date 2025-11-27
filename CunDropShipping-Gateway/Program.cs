using CunDropShipping_Gateway.application.Common;
using CunDropShipping_Gateway.application.Service;
using CunDropShipping_Gateway.domain;
using CunDropShipping_Gateway.domain.Entity;
using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.infrastructure.Entity;
using CunDropShipping_Gateway.infrastructure.Mapper;
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity; 
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Mapper; 

var builder = WebApplication.CreateBuilder(args);

// ==============================================================
// 1. CONFIGURACIÓN DE AMBIENTE Y HERRAMIENTAS
// ==============================================================

// Lee las URLs de los microservicios desde el archivo de configuración (appsettings.json)
var productApiUrl = builder.Configuration.GetValue<string>("ServiceUrls:ProductApi");
var categoryApiUrl = builder.Configuration.GetValue<string>("ServiceUrls:CategoryApi"); 

// Agrega los servicios necesarios para MVC (Controllers), Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==============================================================
// 2. CLIENTES HTTP (CAPA INFRASTRUCTURE)
// ==============================================================
// Registra los clientes HTTP para los microservicios.
// El contenedor de DI inyectará automáticamente el HttpClient configurado
// con la BaseAddress.

builder.Services.AddHttpClient<IProductClient, ProductClient>(client =>
{
    client.BaseAddress = new Uri(productApiUrl);
});

builder.Services.AddHttpClient<ICategoryClient, CategoryClient>(client =>
{
    client.BaseAddress = new Uri(categoryApiUrl);
});

// ==============================================================
// 3. SERVICIOS (CAPA DOMAIN/APPLICATION)
// ==============================================================
// Registra las implementaciones de los servicios bajo sus interfaces de contrato.
// Estos servicios contienen la lógica de orquestación del Gateway.

builder.Services.AddScoped<IProductService, ProductServiceImp>(); 
builder.Services.AddScoped<ICategoryService, CategoryServiceImp>();
builder.Services.AddScoped<IDomainValidatorService, DomainValidatorService>();

// ==============================================================
// 4. MAPPERS (TRADUCTORES GENÉRICOS: IMapper<TIn, TOut>)
// ==============================================================
// Registra las implementaciones concretas para la interfaz genérica IMapper<TIn, TOut>.
// Es CRÍTICO que todas las implementaciones necesarias para construir los servicios
// (Service y Controller) estén presentes aquí.

// --- Flujo de CATEGORIES ---
// 4.1. Mapper Infraestructura (Domain <-> Infrastructure Response)
builder.Services.AddScoped<IMapper<DomainCategoryEntity, CategoryResponse>, CategoryInfrastructureMapper>();

// 4.2. Mapper Adapter (Domain <-> Adapter Dto)
builder.Services.AddScoped<IMapper<DomainCategoryEntity, CategoryDto>, CategoryAdapterMapper>();


// --- Flujo de PRODUCTS (La Corrección del Error) ---
// 4.3. Mapper Infraestructura (Domain <-> Product Response)
// Registra el mapper que el ProductServiceImp necesita para construir su constructor.
// Esto soluciona el error "Unable to resolve service" que estaba ocurriendo.
builder.Services.AddScoped<IMapper<DomainProductEntity, ProductResponse>, ProductInfrastructureMapper>();

// 4.4. Mapper Adapter (Domain <-> Adapter Dto)
// Registra el mapper que el ProductController necesita para serializar la respuesta.
builder.Services.AddScoped<IMapper<DomainProductEntity, ProductDto>, ProductAdapterMapper>();


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
app.MapControllers();
app.Run();