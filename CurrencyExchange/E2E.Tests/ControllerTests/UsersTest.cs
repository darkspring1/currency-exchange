using Api.Controllers;
using BussinesServices.Dto;
using BussinesServices.ServiceResult;
using E2E.Tests.Extensions;
using E2E.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace E2E.Tests.ControllerTests;

public class UsersTest(WebApplicationFactory<Program> factory) : E2EBaseTest(factory)
{
    [Fact]
    public async Task CreateUser_Success()
    {
        var newUser = await Client.CreateRandomUserAsync();
        Assert.NotNull(newUser);

        var loadedUser = await Client.GetUserAsync<UserResponseDto>(newUser.Id);

        Assert.True(newUser.Name.Length > 0);
        Assert.NotEqual(newUser.Id, Guid.Empty);
        Assert.Equal(loadedUser.Id, newUser.Id);
        Assert.Equal(newUser.Name, newUser.Name);
    }

    [Fact]
    public async Task CreateUserBalance_Success()
    {
        const decimal expectedBalance = 100;
        const string currency = "usd";

        var newUser = await Client.CreateRandomUserAsync();
        await Client.CreateUsdAsync();
        Assert.NotNull(newUser);


        void AssertBalance(BalanceResponseDto? bResponse)
        {
            Assert.NotNull(bResponse);
            Assert.Equal(expectedBalance, bResponse.Balance);
            Assert.Equal(currency.ToUpper(), bResponse.CurrencyId);
            Assert.Equal(newUser.Id, bResponse.UserId);
        }

        var dto = new CreateBalanceDto { Balance = expectedBalance };
        var balanceResponse = await Client.CreateUserBalanceAsync<BalanceResponseDto>(newUser.Id.ToString(), currency, dto);

        AssertBalance(balanceResponse);

        balanceResponse = await Client.GetUserBalanceAsync<BalanceResponseDto>(newUser.Id.ToString(), currency);
        AssertBalance(balanceResponse);
    }
    
    [Fact]
    public async Task GetUserBalance_NoExistedUser_NotFound()
    {
        var notExistedUserId = Guid.NewGuid().ToString();
        var response = await Client.GetUserBalanceAsync(notExistedUserId, "usd");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Theory]
    [InlineData(ValidGuid, "u")]
    [InlineData(ValidGuid, "us")]
    [InlineData(ValidGuid, "usdd")]
    public async Task GetUserBalance_Validation(string userId, string currencyId)
    {
        var response = await Client.GetUserBalanceAsync<ServiceError>(userId, currencyId, HttpStatusCode.BadRequest);
        AssertHelpers.ExpectedServiceErrorAsync(response);
    }


    private const string ValidGuid = "CC78522D-CEE8-4EE6-93A5-FD8AB876C666";
}