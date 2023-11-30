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
        
        var dto = new CreateBalanceDto { Balance = expectedBalance };
        var balanceResponse = await Client.CreateUserBalanceAsync<BalanceResponseDto>(newUser.Id.ToString(), currency, dto);

        Assert.NotNull(balanceResponse);
        Assert.Equal(expectedBalance, balanceResponse.Balance);
        Assert.Equal(currency.ToUpper(), balanceResponse.CurrencyId);
        Assert.Equal(newUser.Id, balanceResponse.UserId);
    }

    [Theory]
    [InlineData("CC78522D-CEE8-4EE6-93A5-FD8AB876C666")]
    [InlineData("00000000-0000-0000-0000-AAABBBCCCDDD")]
    public async Task GetUserBalance_Success(Guid userId)
    {
      
        var user = await Client.CreateRandomUserAsync();


        // Act
        var response = await Client.GetAsync($"/users/{userId}/balance");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    
    [Theory]
    //[InlineData("123", "123")]
    [InlineData(ValidGuid, "usd")]
    //[InlineData("", "")]

    //"CC78522D-CEE8-4EE6-93A5-FD8AB876C666"
    public async Task GetUserBalance_Validation(string userId, string currencyId)
    {
        // Act
        var response = await Client.GetUserBalanceAsync<ServiceError>(userId, currencyId, HttpStatusCode.NotFound);

        AssertHelpers.ExpectedServiceErrorAsync(response);
    }


    private const string ValidGuid = "CC78522D-CEE8-4EE6-93A5-FD8AB876C666";
}