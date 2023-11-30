using Api.Controllers;
using BussinesServices.Dto;
using E2E.Tests.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace E2E.Tests.ControllerTests;

public class ExchangeTest(WebApplicationFactory<Program> factory) : E2EBaseTest(factory)
{
    [Fact]
    public async Task Exchange_Success()
    {
        const decimal fromBalance = 100;
        const decimal toBalance = 3;
        const decimal usdToRubRate = 0.011m;
        var newUser = await Client.CreateRandomUserAsync();
        await Task.WhenAll(Client.CreateUsdAsync(), Client.CreateRubAsync());
        
        await Task.WhenAll(
            Client.SetUserBalanceAsync(newUser.Id.ToString(), "usd", fromBalance),
            Client.SetUserBalanceAsync(newUser.Id.ToString(), "rub", toBalance));

        var exchangeResponse = await Client.ExchangeAsync<ExchangeResponseDto>(new ExchangeRequestDto
        {
            IdempotencyKey = Guid.NewGuid(),
            Amount = 100,
            UserId = newUser.Id,
            Fee = 5,
            Rate = usdToRubRate,
            From = "usd",
            To = "rub"
        });

        Assert.NotNull(exchangeResponse);
    }
}