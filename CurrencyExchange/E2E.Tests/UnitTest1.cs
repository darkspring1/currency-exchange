using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;

namespace E2E.Tests;

public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly WebApplicationFactory<Program> _factory;

    public UnitTest1(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Test1()
    {
        var client = _factory.CreateClient();

        //var s = _factory.Server;

        // Act
        var response = await client.GetAsync("/Currencies");
    }
}