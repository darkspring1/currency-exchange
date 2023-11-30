using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace E2E.Tests.ControllerTests;

public class CurrenciesTest : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly WebApplicationFactory<Program> _factory;

    public CurrenciesTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCurrencies_Success()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/currencies");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}