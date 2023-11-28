using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Net;

namespace E2E.Tests.ControllerTests;

public class UserBalanceTest : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly WebApplicationFactory<Program> _factory;

    public UserBalanceTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("CC78522D-CEE8-4EE6-93A5-FD8AB876C666")]
    [InlineData("00000000-0000-0000-0000-AAABBBCCCDDD")]
    public async Task GetUserBalance_Success(Guid userId)
    {
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/users/{userId}/balance");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    [Theory]
    [InlineData("123")]
    [InlineData("00000000-0000-0000-0000-AAABBBCCC@#$")]
    public async Task GetUserBalance_InvalidUserId(string invalidUserId)
    {
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/Users/{invalidUserId}/balance");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}