using System.Net;
using System.Net.Http.Json;
using BussinesServices.Dto;
using BussinesServices.ServiceResult;
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
        Assert.Equal(ToAmount, exchangeResponse.ToAmount);
        Assert.Equal(newUser.Id, exchangeResponse.UserId);
        Assert.Equal("RUB", exchangeResponse.From);
        Assert.Equal("USD", exchangeResponse.To);
        Assert.Equal(Fee, exchangeResponse.Fee);
        Assert.Equal(FeeAmount, exchangeResponse.FeeAmount);
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
        
        AssertBalance(ExpectedFromBalance, "RUB", balances[0]);
        AssertBalance(ExpectedToBalance, "USD", balances[1]);
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
        Assert.Equal(ToAmount, exchangeResponse.ToAmount);
        Assert.Equal(newUser.Id, exchangeResponse.UserId);
        Assert.Equal("RUB", exchangeResponse.From);
        Assert.Equal("USD", exchangeResponse.To);
        Assert.Equal(Fee, exchangeResponse.Fee);
        Assert.Equal(FeeAmount, exchangeResponse.FeeAmount);
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
        
        AssertBalance(ExpectedFromBalance, "RUB", balances[0]);
        AssertBalance(ExpectedToBalance, "USD", balances[1]);
    }
    
    [Fact]
    public async Task Parallel()
    {
        var newUser = await CreateUserAsync();
        
        ExchangeRequestDto Dto()
        {
            return new ExchangeRequestDto
            {
                IdempotencyKey = Guid.NewGuid(),
                Amount = ExchangeAmount,
                UserId = newUser.Id,
                Fee = Fee,
                Rate = Rate,
                From = "rub",
                To = "usd"
            };
        }
        
        await Task.WhenAll(
            Client.ExchangeAsync<ExchangeResponseDto>(Dto()),
            Client.ExchangeAsync<ExchangeResponseDto>(Dto()),
            Client.ExchangeAsync<ExchangeResponseDto>(Dto()));
    }
    
    [Fact]
    public async Task Parallel_BalanceIsNot()
    {
        var expectedErrorCode = Errors.SmallBalance(0, "", 0).Code;
        
        var newUser = await CreateUserAsync(ExchangeAmount*2);
        
        ExchangeRequestDto Dto()
        {
            return new ExchangeRequestDto
            {
                IdempotencyKey = Guid.NewGuid(),
                Amount = ExchangeAmount,
                UserId = newUser.Id,
                Fee = Fee,
                Rate = Rate,
                From = "rub",
                To = "usd"
            };
        }
        
        var responses = await Task.WhenAll(
            Client.ExchangeAsync(Dto()),
            Client.ExchangeAsync(Dto()),
            Client.ExchangeAsync(Dto()));
        
        var badResponse = responses.Single(r => r.StatusCode == HttpStatusCode.BadRequest);
        var actualError = await badResponse.Content.ReadFromJsonAsync<ServiceError>();
        
        Assert.Equal(2, responses.Count(r => r.StatusCode == HttpStatusCode.OK));
        Assert.Equal(expectedErrorCode, actualError.Code);
    }

    private async Task<UserResponseDto> CreateUserAsync(decimal fromBalance = 1000)
    {
        var newUser = await Client.CreateRandomUserAsync();
        await Task.WhenAll(Client.CreateUsdAsync(), Client.CreateRubAsync());
        
        await Task.WhenAll(
            Client.SetUserBalanceAsync(newUser.Id.ToString(), "rub", fromBalance),
            Client.SetUserBalanceAsync(newUser.Id.ToString(), "usd", ToBalance));

        return newUser;
    }
    
    const decimal ToBalance = 3;
    const decimal Rate = 0.011m; //rub -> usd
    const decimal Fee = 0.05m;
    const decimal ExchangeAmount = 100;
        
    const decimal ExpectedFromBalance = 1000 - ExchangeAmount;
    const decimal ToAmount = ExchangeAmount * Rate;
    const decimal FeeAmount = ToAmount * Fee;

    const decimal ExpectedToBalance = ToBalance + ToAmount - FeeAmount;
}