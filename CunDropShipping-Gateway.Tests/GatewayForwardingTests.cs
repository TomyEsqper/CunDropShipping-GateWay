using System.Net;
using System.Text;
using CunDropShipping_Gateway.Tests.Infrastructure;
using Xunit;

namespace CunDropShipping_Gateway.Tests;

public sealed class GatewayForwardingTests
{
    [Fact]
    public async Task GetProducts_ForwardsToCatalogProductsEndpoint()
    {
        using var factory = new GatewayWebApplicationFactory
        {
            CatalogHandler = new StubHttpMessageHandler(_ =>
                StubHttpMessageHandler.Json(HttpStatusCode.OK, """[{"idProduct":1,"nameProduct":"Keyboard"}]"""))
        };
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/gateway/v1/products");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("""[{"idProduct":1,"nameProduct":"Keyboard"}]""", await response.Content.ReadAsStringAsync());
        Assert.Single(factory.CatalogHandler.Requests);
        Assert.Equal(HttpMethod.Get, factory.CatalogHandler.Requests[0].Method);
        Assert.Equal("http://catalog.test/api/v1/products", factory.CatalogHandler.Requests[0].RequestUri?.ToString());
    }

    [Fact]
    public async Task PostProducts_ForwardsBodyToCatalogProductsEndpoint()
    {
        using var factory = new GatewayWebApplicationFactory
        {
            CatalogHandler = new StubHttpMessageHandler(_ =>
                StubHttpMessageHandler.Json(HttpStatusCode.Created, """{"idProduct":8}"""))
        };
        using var client = factory.CreateClient();

        var response = await client.PostAsync(
            "/api/gateway/v1/products",
            new StringContent("""{"nameProduct":"Mouse"}""", Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Single(factory.CatalogHandler.Requests);
        Assert.Equal("http://catalog.test/api/v1/products", factory.CatalogHandler.Requests[0].RequestUri?.ToString());
        Assert.Equal("""{"nameProduct":"Mouse"}""", await factory.CatalogHandler.Requests[0].Content!.ReadAsStringAsync());
    }

    [Fact]
    public async Task GetCategories_ForwardsToCatalogCategoriesEndpoint()
    {
        using var factory = new GatewayWebApplicationFactory
        {
            CatalogHandler = new StubHttpMessageHandler(_ =>
                StubHttpMessageHandler.Json(HttpStatusCode.OK, """[{"categoryId":2,"nameCategory":"Tech"}]"""))
        };
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/gateway/v1/categories");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("http://catalog.test/api/v1/categories", factory.CatalogHandler.Requests.Single().RequestUri?.ToString());
    }

    [Fact]
    public async Task PostCategories_ForwardsBodyToCatalogCategoriesEndpoint()
    {
        using var factory = new GatewayWebApplicationFactory
        {
            CatalogHandler = new StubHttpMessageHandler(_ =>
                StubHttpMessageHandler.Json(HttpStatusCode.Created, """{"categoryId":2}"""))
        };
        using var client = factory.CreateClient();

        var response = await client.PostAsync(
            "/api/gateway/v1/categories",
            new StringContent("""{"nameCategory":"Tech"}""", Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("http://catalog.test/api/v1/categories", factory.CatalogHandler.Requests.Single().RequestUri?.ToString());
    }

    [Fact]
    public async Task GetSubcategories_ForwardsToCatalogSubcategoriesEndpoint()
    {
        using var factory = new GatewayWebApplicationFactory
        {
            CatalogHandler = new StubHttpMessageHandler(_ =>
                StubHttpMessageHandler.Json(HttpStatusCode.OK, """[{"subCategoryId":4,"nameSubCategory":"Laptops"}]"""))
        };
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/gateway/v1/subcategories");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("http://catalog.test/api/v1/subcategories", factory.CatalogHandler.Requests.Single().RequestUri?.ToString());
    }

    [Fact]
    public async Task PostSubcategories_ForwardsBodyToCatalogSubcategoriesEndpoint()
    {
        using var factory = new GatewayWebApplicationFactory
        {
            CatalogHandler = new StubHttpMessageHandler(_ =>
                StubHttpMessageHandler.Json(HttpStatusCode.Created, """{"subCategoryId":4}"""))
        };
        using var client = factory.CreateClient();

        var response = await client.PostAsync(
            "/api/gateway/v1/subcategories",
            new StringContent("""{"nameSubCategory":"Laptops","categoryId":2}""", Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("http://catalog.test/api/v1/subcategories", factory.CatalogHandler.Requests.Single().RequestUri?.ToString());
    }

    [Fact]
    public async Task GetCartByUserId_ForwardsToCartBuyerEndpoint()
    {
        var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        using var factory = new GatewayWebApplicationFactory
        {
            CartHandler = new StubHttpMessageHandler(_ =>
                StubHttpMessageHandler.Json(HttpStatusCode.OK, """{"shoppingCartId":"22222222-2222-2222-2222-222222222222"}"""))
        };
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"/api/gateway/v1/carts/{userId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal($"http://cart.test/carts/buyer/{userId}", factory.CartHandler.Requests.Single().RequestUri?.ToString());
    }

    [Fact]
    public async Task PostCarts_ForwardsBodyToCartCreateEndpoint()
    {
        using var factory = new GatewayWebApplicationFactory
        {
            CartHandler = new StubHttpMessageHandler(_ =>
                StubHttpMessageHandler.Json(HttpStatusCode.Created, """{"shoppingCartId":"22222222-2222-2222-2222-222222222222"}"""))
        };
        using var client = factory.CreateClient();

        var response = await client.PostAsync(
            "/api/gateway/v1/carts",
            new StringContent("""{"buyerId":"11111111-1111-1111-1111-111111111111"}""", Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("http://cart.test/carts", factory.CartHandler.Requests.Single().RequestUri?.ToString());
    }
}
