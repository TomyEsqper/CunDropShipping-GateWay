using System.Text;
using System.Text.Json;

namespace CunDropShipping_Gateway.infrastructure.Clients;

public abstract class GatewayHttpClientBase
{
    private readonly HttpClient _httpClient;

    protected GatewayHttpClientBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<GatewayResponse> GetAsync(string path, CancellationToken cancellationToken)
    {
        return SendAsync(new HttpRequestMessage(HttpMethod.Get, path), cancellationToken);
    }

    public Task<GatewayResponse> PostAsync<T>(string path, T body, CancellationToken cancellationToken)
    {
        return SendAsync(CreateJsonMessage(HttpMethod.Post, path, body), cancellationToken);
    }

    public async Task<GatewayResponse> PostAsync(string path, HttpRequest request, CancellationToken cancellationToken)
    {
        request.EnableBuffering();
        request.Body.Position = 0;

        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync(cancellationToken);
        request.Body.Position = 0;

        var content = new StringContent(body, Encoding.UTF8);
        if (!string.IsNullOrWhiteSpace(request.ContentType))
        {
            content.Headers.TryAddWithoutValidation("Content-Type", request.ContentType);
        }
        else
        {
            content.Headers.TryAddWithoutValidation("Content-Type", "application/json");
        }

        var message = new HttpRequestMessage(HttpMethod.Post, path)
        {
            Content = content
        };

        return await SendAsync(message, cancellationToken);
    }

    public Task<GatewayResponse> PutAsync<T>(string path, T body, CancellationToken cancellationToken)
    {
        return SendAsync(CreateJsonMessage(HttpMethod.Put, path, body), cancellationToken);
    }

    public async Task<GatewayResponse> PutAsync(string path, HttpRequest request, CancellationToken cancellationToken)
    {
        request.EnableBuffering();
        request.Body.Position = 0;

        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync(cancellationToken);
        request.Body.Position = 0;

        var content = new StringContent(body, Encoding.UTF8);
        if (!string.IsNullOrWhiteSpace(request.ContentType))
        {
            content.Headers.TryAddWithoutValidation("Content-Type", request.ContentType);
        }
        else
        {
            content.Headers.TryAddWithoutValidation("Content-Type", "application/json");
        }

        var message = new HttpRequestMessage(HttpMethod.Put, path)
        {
            Content = content
        };

        return await SendAsync(message, cancellationToken);
    }

    public Task<GatewayResponse> DeleteAsync(string path, CancellationToken cancellationToken)
    {
        return SendAsync(new HttpRequestMessage(HttpMethod.Delete, path), cancellationToken);
    }

    public Task<GatewayResponse> PatchAsync<T>(string path, T body, CancellationToken cancellationToken)
    {
        return SendAsync(CreateJsonMessage(HttpMethod.Patch, path, body), cancellationToken);
    }

    public async Task<GatewayResponse> PatchAsync(string path, HttpRequest request, CancellationToken cancellationToken)
    {
        request.EnableBuffering();
        request.Body.Position = 0;

        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync(cancellationToken);
        request.Body.Position = 0;

        var content = new StringContent(body, Encoding.UTF8);
        if (!string.IsNullOrWhiteSpace(request.ContentType))
        {
            content.Headers.TryAddWithoutValidation("Content-Type", request.ContentType);
        }
        else
        {
            content.Headers.TryAddWithoutValidation("Content-Type", "application/json");
        }

        var message = new HttpRequestMessage(HttpMethod.Patch, path)
        {
            Content = content
        };

        return await SendAsync(message, cancellationToken);
    }

    private static HttpRequestMessage CreateJsonMessage<T>(HttpMethod method, string path, T body)
    {
        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return new HttpRequestMessage(method, path)
        {
            Content = content
        };
    }

    private async Task<GatewayResponse> SendAsync(HttpRequestMessage message, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.SendAsync(message, cancellationToken);
        var content = response.Content is null
            ? string.Empty
            : await response.Content.ReadAsStringAsync(cancellationToken);

        return new GatewayResponse(
            response.StatusCode,
            content,
            response.Content?.Headers.ContentType?.ToString());
    }
}
