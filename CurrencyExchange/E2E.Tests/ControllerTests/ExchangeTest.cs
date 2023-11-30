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
        const decimal fromBalance = 1000;
        const decimal toBalance = 3;
        const decimal rate = 0.011m; //rub -> usd
        const decimal fee = 0.05m;
        const decimal exchangeAmount = 100;
        
        const decimal expectedFromBalance = 1000 - exchangeAmount;
        const decimal toAmount = exchangeAmount * rate;
        const decimal feeAmount = toAmount*fee;

        const decimal expectedToBalance = toBalance + toAmount - feeAmount;
        
        
        var newUser = await Client.CreateRandomUserAsync();
        await Task.WhenAll(Client.CreateUsdAsync(), Client.CreateRubAsync());
        
        await Task.WhenAll(
            Client.SetUserBalanceAsync(newUser.Id.ToString(), "rub", fromBalance),
            Client.SetUserBalanceAsync(newUser.Id.ToString(), "usd", toBalance));

        var idempotencyKey = Guid.NewGuid();
        var exchangeResponse = await Client.ExchangeAsync<ExchangeResponseDto>(new ExchangeRequestDto
        {
            IdempotencyKey = idempotencyKey,
            Amount = exchangeAmount,
            UserId = newUser.Id,
            Fee = fee,
            Rate = rate,
            From = "rub",
            To = "usd"
        });

        Assert.NotNull(exchangeResponse);
        Assert.Equal(idempotencyKey, exchangeResponse.IdempotencyKey);
        Assert.Equal(exchangeAmount, exchangeResponse.FromAmount);
        Assert.Equal(toAmount, exchangeResponse.ToAmount);
        Assert.Equal(newUser.Id, exchangeResponse.UserId);
        Assert.Equal("RUB", exchangeResponse.From);
        Assert.Equal("USD", exchangeResponse.To);
        Assert.Equal(fee, exchangeResponse.Fee);
        Assert.Equal(feeAmount, exchangeResponse.FeeAmount);
        Assert.Equal(rate, exchangeResponse.Rate);
        
        var balances = await Task.WhenAll(
            Client.GetUserBalanceAsync<BalanceResponseDto>(newUser.Id.ToString(), "rub"),
            Client.GetUserBalanceAsync<BalanceResponseDto>(newUser.Id.ToString(), "usd"));

        void AssertBalance(decimal expectedBalance, string expectedCurrencyId, BalanceResponseDto? bDto)
        {
            Assert.NotNull(bDto);
            Assert.Equal(expectedBalance, bDto.Balance);
            Assert.Equal(expectedCurrencyId, bDto.CurrencyId);
            Assert.Equal(newUser.Id, bDto.UserId);
        }
        
        AssertBalance(expectedFromBalance, "RUB", balances[0]);
        AssertBalance(expectedToBalance, "USD", balances[1]);
    }
}