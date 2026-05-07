using CunDropShipping_Gateway.infrastructure.Clients;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

internal static class GatewayResultFactory
{
    public static async Task<IActionResult> CreateAsync(
        ControllerBase controller,
        Task<GatewayResponse> responseTask)
    {
        var response = await responseTask;

        if (string.IsNullOrWhiteSpace(response.Content))
        {
            return controller.StatusCode((int)response.StatusCode);
        }

        return new ContentResult
        {
            StatusCode = (int)response.StatusCode,
            Content = response.Content,
            ContentType = response.ContentType
        };
    }
}
