using System.Text.Json;
using System.Net.Http.Json;
using CunDropShipping_Gateway.infrastructure.Entity; // Único using de entidades

namespace CunDropShipping_Gateway.infrastructure.Clients
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ProductClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public List<ProductResponse> GetAllProducts()
        {
            // .Result bloquea el hilo, idealmente usa async/await, pero lo dejo como lo tienes
            var response = _httpClient
                .GetFromJsonAsync<List<ProductResponse>>("/api/v1/products", _jsonOptions)
                .Result;

            return response ?? new List<ProductResponse>();
        }

        public ProductResponse GetProductById(int idProduct)
        {
            var response = _httpClient
                .GetFromJsonAsync<ProductResponse>($"/api/v1/products/{idProduct}", _jsonOptions)
                .Result;
                
            return response;
        }

        // ✅ CORREGIDO: Recibe ProductResponse y lo envía como JSON
        public ProductResponse SaveProduct(ProductResponse product)
        {
            // Enviamos el objeto 'ProductResponse' tal cual
            var response = _httpClient.PostAsJsonAsync("/api/v1/products", product).Result;
            
            response.EnsureSuccessStatusCode(); 

            return response.Content.ReadFromJsonAsync<ProductResponse>(_jsonOptions).Result;
        }

        // ✅ CORREGIDO: Recibe ProductResponse y lo envía como JSON
        public ProductResponse UpdateProduct(int idProduct, ProductResponse product)
        {
            var response = _httpClient.PutAsJsonAsync($"/api/v1/products/{idProduct}", product).Result;
            
            response.EnsureSuccessStatusCode();

            return response.Content.ReadFromJsonAsync<ProductResponse>(_jsonOptions).Result;
        }

        public ProductResponse DeleteProduct(int idProduct)
        {
            var response = _httpClient.DeleteAsync($"/api/v1/products/{idProduct}").Result;
            
            response.EnsureSuccessStatusCode();

            return response.Content.ReadFromJsonAsync<ProductResponse>(_jsonOptions).Result;
        }

        public List<ProductResponse> SearchProductsByName(string searchTerm)
        {
            var response = _httpClient
                .GetFromJsonAsync<List<ProductResponse>>($"/api/v1/products/search?searchTerm={searchTerm}", _jsonOptions)
                .Result;

            return response ?? new List<ProductResponse>();
        }

        public List<ProductResponse> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return _httpClient
                .GetFromJsonAsync<List<ProductResponse>>(
                    $"/api/v1/products/filter/price?minPrice={minPrice}&maxPrice={maxPrice}", _jsonOptions)
                .Result;
        }

        public List<ProductResponse> GetProductsWithLowStock(int stockThreshold)
        {
            var response = _httpClient
                .GetFromJsonAsync<List<ProductResponse>>($"/api/v1/products/filter/stock?stockThreshold={stockThreshold}", _jsonOptions)
                .Result;

            return response ?? new List<ProductResponse>();
        }
    }
}