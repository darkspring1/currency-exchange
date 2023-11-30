using BussinesServices.Dto;
using E2E.Tests.Extensions;
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

    [Theory]
    [InlineData("CC78522D-CEE8-4EE6-93A5-FD8AB876C666")]
    [InlineData("00000000-0000-0000-0000-AAABBBCCCDDD")]
    public async Task GetUserBalance_Success(Guid userId)
    {
      

        // Act
        var response = await Client.GetAsync($"/users/{userId}/balance");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    
    [Theory]
    [InlineData("123")]
    [InlineData("00000000-0000-0000-0000-AAABBBCCC@#$")]
    public async Task GetUserBalance_InvalidUserId(string invalidUserId)
    {
       

        // Act
        var response = await Client.GetAsync($"/Users/{invalidUserId}/balance");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}