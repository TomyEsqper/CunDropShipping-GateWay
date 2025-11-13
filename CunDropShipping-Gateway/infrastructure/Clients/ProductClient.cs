using System.Text.Json;
using System.Net.Http.Json;
using CunDropShipping_Gateway.infrastructure.Entity;

namespace CunDropShipping_Gateway.infrastructure.Clients;

public class ProductClient : IProductClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ProductClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // Configuramos el JSON para que no le importen mayusculas y minusculas.
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
    
    public List<ProductResponse> GetAllProducts()
    {
        // ¡Aquí está la magia! Hacemos la llamada HTTP.
        // Usamos .Result para que sea SÍNCRONO (como tus otros micros)
        var response = _httpClient
            .GetFromJsonAsync<List<ProductResponse>>("/api/v1/products", _jsonOptions)
            .Result;
        
        return response ?? new List<ProductResponse>();
    }

    public ProductResponse GetProductById(int IdProduct)
    {
        var response = _httpClient
            .GetFromJsonAsync<ProductResponse>($"/api/v1/products/{IdProduct}", _jsonOptions)
            .Result;
        return response;
    }

    public ProductResponse SaveProduct(ProductRequest request)
    {
        var response = _httpClient
            .PostAsJsonAsync("/api/v1/products", request, _jsonOptions)
            .Result;
        response.EnsureSuccessStatusCode();
        return response.Content.ReadFromJsonAsync<ProductResponse>(_jsonOptions).Result!;
    }

    public ProductResponse UpdateProduct(int IdProduct, ProductRequest request)
    {
        var response = _httpClient
            .PutAsJsonAsync($"/api/v1/products/{IdProduct}", request, _jsonOptions)
            .Result;
        response.EnsureSuccessStatusCode();
        return response.Content.ReadFromJsonAsync<ProductResponse>(_jsonOptions).Result!;
    }

    public ProductResponse DeleteProduct(int IdProduct)
    {
        var response = _httpClient.DeleteAsync($"/api/v1/products/{IdProduct}").Result;
        response.EnsureSuccessStatusCode();
        return response.Content.ReadFromJsonAsync<ProductResponse>(_jsonOptions).Result!;
    }

    public List<ProductResponse> SearchProductsByName(string searchTerm)
    {
        var response = _httpClient
            .GetFromJsonAsync<List<ProductResponse>>($"/api/v1/products/search?searchTerm={Uri.EscapeDataString(searchTerm)}", _jsonOptions)
            .Result;
        return response ?? new List<ProductResponse>();
    }

    public List<ProductResponse> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
    {
        var response = _httpClient
            .GetFromJsonAsync<List<ProductResponse>>($"/api/v1/products/filter/price?minPrice={minPrice}&max={maxPrice}", _jsonOptions)
            .Result;
        return response ?? new List<ProductResponse>();
    }

    public List<ProductResponse> GetProductsWithLowStock(int stockThreshold)
    {
        var response = _httpClient
            .GetFromJsonAsync<List<ProductResponse>>($"/api/v1/products/filter/stock?stockThreshold={stockThreshold}", _jsonOptions)
            .Result;
        return response ?? new List<ProductResponse>();
    }
}