using System.Text.Json;
using System.Net.Http.Json;
using CunDropShipping_Gateway.infrastructure.Entity; 

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

        // [EDUCATIVO] Agregamos 'async' para poder usar 'await' dentro.
        // El retorno ahora es Task<List<ProductResponse>>.
        public async Task<List<ProductResponse>> GetAllProducts()
        {
            // [EDUCATIVO] Eliminamos .Result (que bloquea) y usamos 'await'.
            // 'await' libera este hilo para que atienda otras peticiones mientras
            // la respuesta viaja por la red.
            var httpResponse = await _httpClient.GetAsync("/api/v1/products");
            httpResponse.EnsureSuccessStatusCode();

            // [EDUCATIVO] ReadAsStringAsync también es una operación I/O, así que la esperamos.
            var rawJson = await httpResponse.Content.ReadAsStringAsync();
            
            // Console.WriteLine(rawJson); // Comentado por limpieza, reactivar si se necesita debug

            try
            {
                var list = JsonSerializer.Deserialize<List<ProductResponse>>(rawJson, _jsonOptions);
                return list ?? new List<ProductResponse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
                throw;
            }
        }

        public async Task<ProductResponse?> GetProductById(int idProduct)
        {
            // [EDUCATIVO] GetFromJsonAsync es una joya: hace el Get, valida el status, lee el body
            // y deserializa, todo optimizado y asíncrono. ¡Mucho más limpio!
            var response = await _httpClient
                .GetFromJsonAsync<ProductResponse>($"/api/v1/products/{idProduct}", _jsonOptions);
                
            return response;
        }

        public async Task<ProductResponse?> SaveProduct(ProductResponse product)
        {
            // [EDUCATIVO] PostAsJsonAsync envía el objeto serializado de forma asíncrona.
            var response = await _httpClient.PostAsJsonAsync("/api/v1/products", product);
            
            response.EnsureSuccessStatusCode(); 

            // [EDUCATIVO] Leemos la respuesta (el producto creado) asíncronamente.
            return await response.Content.ReadFromJsonAsync<ProductResponse>(_jsonOptions);
        }

        public async Task<ProductResponse?> UpdateProduct(int idProduct, ProductResponse product)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/v1/products/{idProduct}", product);
            
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ProductResponse>(_jsonOptions);
        }

        public async Task<ProductResponse?> DeleteProduct(int idProduct)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/products/{idProduct}");
            
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ProductResponse>(_jsonOptions);
        }

        public async Task<List<ProductResponse>> SearchProductsByName(string searchTerm)
        {
            var response = await _httpClient
                .GetFromJsonAsync<List<ProductResponse>>($"/api/v1/products/search?searchTerm={searchTerm}", _jsonOptions);

            return response ?? new List<ProductResponse>();
        }

        public async Task<List<ProductResponse>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return await _httpClient
                .GetFromJsonAsync<List<ProductResponse>>(
                    $"/api/v1/products/filter/price?minPrice={minPrice}&maxPrice={maxPrice}", _jsonOptions);
        }

        public async Task<List<ProductResponse>> GetProductsWithLowStock(int stockThreshold)
        {
            var response = await _httpClient
                .GetFromJsonAsync<List<ProductResponse>>($"/api/v1/products/filter/stock?stockThreshold={stockThreshold}", _jsonOptions);

            return response ?? new List<ProductResponse>();
        }
    }
}
