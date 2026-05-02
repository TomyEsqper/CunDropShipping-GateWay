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
            // Paso 1: Obtener y loguear el JSON crudo para inspeccionar los nombres de las propiedades
            var httpResponse = _httpClient.GetAsync("/api/v1/products").Result;
            httpResponse.EnsureSuccessStatusCode();

            var rawJson = httpResponse.Content.ReadAsStringAsync().Result;
            Console.WriteLine("[DEBUG_LOG][ProductClient.GetAllProducts] Raw JSON payload:");
            Console.WriteLine(rawJson);

            // Paso 2: Deserializar usando opciones con PropertyNameCaseInsensitive = true
            try
            {
                var list = JsonSerializer.Deserialize<List<ProductResponse>>(rawJson, _jsonOptions);
                return list ?? new List<ProductResponse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG_LOG][ProductClient.GetAllProducts] Error deserializando respuesta: {ex.Message}");
                throw;
            }
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