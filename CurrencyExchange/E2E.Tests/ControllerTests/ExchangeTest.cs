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
        var newUser = await Client.CreateRandomUserAsync();
        await Task.WhenAll(Client.CreateUsdAsync(), Client.CreateRubAsync());
        
        await Task.WhenAll(
            Client.SetUserBalanceAsync(newUser.Id.ToString(), "usd", fromBalance),
            Client.SetUserBalanceAsync(newUser.Id.ToString(), "rub", toBalance));
        
        // Assert.NotNull(newUser);
        //
        // var loadedUser = await Client.GetUserAsync<UserResponseDto>(newUser.Id);
        //
        // Assert.True(newUser.Name.Length > 0);
        // Assert.NotEqual(newUser.Id, Guid.Empty);
        // Assert.Equal(loadedUser.Id, newUser.Id);
        // Assert.Equal(newUser.Name, newUser.Name);
    }
}