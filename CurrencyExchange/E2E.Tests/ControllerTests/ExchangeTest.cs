using BussinesServices.Dto;
using E2E.Tests.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace E2E.Tests.ControllerTests;

public class ExchangeTest(WebApplicationFactory<Program> factory) : E2EBaseTest(factory)
{
    [Fact]
    public async Task Success()
    {
        var newUser = await CreateUserAsync();
        var idempotencyKey = Guid.NewGuid();
        var exchangeResponse = await Client.ExchangeAsync<ExchangeResponseDto>(new ExchangeRequestDto
        {
            IdempotencyKey = idempotencyKey,
            Amount = ExchangeAmount,
            UserId = newUser.Id,
            Fee = Fee,
            Rate = Rate,
            From = "rub",
            To = "usd"
        });

        Assert.NotNull(exchangeResponse);
        Assert.Equal(idempotencyKey, exchangeResponse.IdempotencyKey);
        Assert.Equal(ExchangeAmount, exchangeResponse.FromAmount);
        Assert.Equal(toAmount, exchangeResponse.ToAmount);
        Assert.Equal(newUser.Id, exchangeResponse.UserId);
        Assert.Equal("RUB", exchangeResponse.From);
        Assert.Equal("USD", exchangeResponse.To);
        Assert.Equal(Fee, exchangeResponse.Fee);
        Assert.Equal(feeAmount, exchangeResponse.FeeAmount);
        Assert.Equal(Rate, exchangeResponse.Rate);
        
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
    
    [Fact]
    public async Task Idempotency()
    {
        var newUser = await CreateUserAsync();
        var idempotencyKey = Guid.NewGuid();
        var dto = new ExchangeRequestDto
        {
            IdempotencyKey = idempotencyKey,
            Amount = ExchangeAmount,
            UserId = newUser.Id,
            Fee = Fee,
            Rate = Rate,
            From = "rub",
            To = "usd"
        };
        var exchangeResponse = await Client.ExchangeAsync<ExchangeResponseDto>(dto);
        exchangeResponse = await Client.ExchangeAsync<ExchangeResponseDto>(dto);

        Assert.NotNull(exchangeResponse);
        Assert.Equal(idempotencyKey, exchangeResponse.IdempotencyKey);
        Assert.Equal(ExchangeAmount, exchangeResponse.FromAmount);
        Assert.Equal(toAmount, exchangeResponse.ToAmount);
        Assert.Equal(newUser.Id, exchangeResponse.UserId);
        Assert.Equal("RUB", exchangeResponse.From);
        Assert.Equal("USD", exchangeResponse.To);
        Assert.Equal(Fee, exchangeResponse.Fee);
        Assert.Equal(feeAmount, exchangeResponse.FeeAmount);
        Assert.Equal(Rate, exchangeResponse.Rate);
        
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

    private async Task<UserResponseDto> CreateUserAsync()
    {
        var newUser = await Client.CreateRandomUserAsync();
        await Task.WhenAll(Client.CreateUsdAsync(), Client.CreateRubAsync());
        
        await Task.WhenAll(
            Client.SetUserBalanceAsync(newUser.Id.ToString(), "rub", FromBalance),
            Client.SetUserBalanceAsync(newUser.Id.ToString(), "usd", ToBalance));

        return newUser;
    }

    const decimal FromBalance = 1000;
    const decimal ToBalance = 3;
    const decimal Rate = 0.011m; //rub -> usd
    const decimal Fee = 0.05m;
    const decimal ExchangeAmount = 100;
        
    const decimal expectedFromBalance = 1000 - ExchangeAmount;
    const decimal toAmount = ExchangeAmount * Rate;
    const decimal feeAmount = toAmount * Fee;

    const decimal expectedToBalance = ToBalance + toAmount - feeAmount;
}