using System.Text.Json;
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
    
    public List<CategoryResponse> GetAllCategories()
    {
        var response = _httpClient.
            GetFromJsonAsync<List<CategoryResponse>>("/api/v1/categories", _jsonOptions)
            .Result;
        
        return response ?? new List<CategoryResponse>();
    }

    public List<CategoryResponse>? GetCategoriesByName(string name)
    {
        var response = _httpClient.
            GetFromJsonAsync<List<CategoryResponse>>($"/api/v1/Categories/ByName/{name}", _jsonOptions)
            .Result;
        return response ?? new List<CategoryResponse>();
    }

    public CategoryResponse? GetCategoryById(int id)
    {
        var response = _httpClient.GetFromJsonAsync<CategoryResponse>($"/api/v1/categories/{id}", _jsonOptions)
            .Result;
        return response;
    }

    public CategoryResponse CreateCategory(CategoryResponse category)
    {
        var response = _httpClient.PostAsJsonAsync("/api/v1/categories", category).Result;
        
        response.EnsureSuccessStatusCode();
        
        return response.Content.ReadFromJsonAsync<CategoryResponse>(_jsonOptions).Result;
    }

    public CategoryResponse? UpdateCategory(int id, CategoryResponse category)
    {
        var response = _httpClient.PutAsJsonAsync($"/api/v1/categories/{id}", category).Result;
        response.EnsureSuccessStatusCode();
        
        return response.Content.ReadFromJsonAsync<CategoryResponse>(_jsonOptions).Result;
    }

    public CategoryResponse? DeleteCategoryById(int id)
    {
        var respone = _httpClient.DeleteAsync($"/api/v1/Categories/{id}").Result;
        
        respone.EnsureSuccessStatusCode();
        
        return respone.Content.ReadFromJsonAsync<CategoryResponse>(_jsonOptions).Result;
    }

    public List<CategoryResponse>? DeleteCategoryByName(string name)
    {
        var response = _httpClient.DeleteAsync($"/api/v1/Categories/ByName/{name}").Result;
        
        response.EnsureSuccessStatusCode();
        
        return response.Content.ReadFromJsonAsync<List<CategoryResponse>>(_jsonOptions).Result;
    }
}