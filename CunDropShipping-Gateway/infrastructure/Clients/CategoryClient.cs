using System.Text.Json;
using System.Net.Http.Json;
using CunDropShipping_Gateway.infrastructure.Entity;

namespace CunDropShipping_Gateway.infrastructure.Clients;

public class CategoryClient : ICategoryClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    
    public CategoryClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
    
    public async Task<List<CategoryResponse>> GetAllCategories()
    {
        // [EDUCATIVO] await libera el hilo mientras esperamos la respuesta de red.
        var response = await _httpClient.GetFromJsonAsync<List<CategoryResponse>>("/api/v1/categories", _jsonOptions);
        return response ?? new List<CategoryResponse>();
    }

    public async Task<List<CategoryResponse>> GetCategoriesByName(string name)
    {
        var response = await _httpClient.GetFromJsonAsync<List<CategoryResponse>>($"/api/v1/Categories/ByName/{name}", _jsonOptions);
        return response ?? new List<CategoryResponse>();
    }

    public async Task<CategoryResponse> GetCategoryById(int id)
    {
        // [EDUCATIVO] Si el API devuelve 404, GetFromJsonAsync lanza excepción.
        // Manejaremos excepciones globalmente o con try-catch si es necesario.
        return await _httpClient.GetFromJsonAsync<CategoryResponse>($"/api/v1/categories/{id}", _jsonOptions);
    }

    public async Task<CategoryResponse> CreateCategory(CategoryResponse category)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/v1/categories", category);
        
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<CategoryResponse>(_jsonOptions);
    }

    public async Task<CategoryResponse> UpdateCategory(int id, CategoryResponse category)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/v1/categories/{id}", category);
        
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<CategoryResponse>(_jsonOptions);
    }

    public async Task<CategoryResponse> DeleteCategoryById(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/v1/Categories/{id}");
        
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<CategoryResponse>(_jsonOptions);
    }

    public async Task<List<CategoryResponse>> DeleteCategoryByName(string name)
    {
        var response = await _httpClient.DeleteAsync($"/api/v1/Categories/ByName/{name}");
        
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<List<CategoryResponse>>(_jsonOptions);
    }
}
