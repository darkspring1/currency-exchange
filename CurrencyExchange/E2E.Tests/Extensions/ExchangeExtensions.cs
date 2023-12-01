using System.Net;
using System.Net.Http.Json;
using BussinesServices.Dto;

namespace E2E.Tests.Extensions;

internal static class ExchangeExtensions
{
    public static async Task<T?> ExchangeAsync<T>(this HttpClient client, ExchangeRequestDto dto)
    {
        var response = await client.PostAsJsonAsync("/exchange", dto);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        return await response.Content.ReadFromJsonAsync<T>();
    }
    
    public static Task<HttpResponseMessage> ExchangeAsync(this HttpClient client, ExchangeRequestDto dto)
    {
        return client.PostAsJsonAsync("/exchange", dto);
    }
}