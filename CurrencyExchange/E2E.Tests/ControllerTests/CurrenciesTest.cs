using BussinesServices.Dto;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using E2E.Tests.Extensions;
using E2E.Tests.Helpers;
using BussinesServices.ServiceResult;

namespace E2E.Tests.ControllerTests;

public class CurrenciesTest : IClassFixture<WebApplicationFactory<Program>>
{
    public CurrenciesTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCurrencies_Success()
    {
        const string id = "usd";
        const string name = "United States Dollar";

        var expected = await Client.CreateCurrencyAsync<CurrencyResponseDto>(new CreateCurrencyDto { Id = id, Name = name });
        var actual = await Client.GetCurrencyAsync<CurrencyResponseDto>(id, HttpStatusCode.OK);

        Assert.NotNull(expected);
        Assert.NotNull(actual);
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Name, actual.Name);
    }

    [Fact]
    public async Task CreateCurrency_Success()
    {
        var dto = await Client.CreateRandomCurrencyAsync();
        
        Assert.NotNull(dto);
        Assert.True(dto.Id.IsUpper());
        Assert.True(dto.Id.Length == 3);
        Assert.True(dto.Name.Length > 0);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("USD", null)]
    [InlineData("USD", "")]
    [InlineData("USD", " ")]
    [InlineData("longName", "")]
    [InlineData("USD", MoreThan100SymbolsString)]
    [InlineData("@#$", "")]
    [InlineData("&*_", "")]
    public async Task CreateCurrency_Validation(string id, string name)
    {
        var response = await Client.CreateCurrencyAsync<ServiceError>(new CreateCurrencyDto { Id = id, Name = name }, HttpStatusCode.BadRequest);
        AssertHelpers.ExpectedServiceErrorAsync(response);
    }

    private HttpClient Client => _factory.CreateClient();
    private const string MoreThan100SymbolsString = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901";
    private readonly WebApplicationFactory<Program> _factory;
}