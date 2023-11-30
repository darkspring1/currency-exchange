using Microsoft.AspNetCore.Mvc.Testing;

namespace E2E.Tests.ControllerTests;

public class E2EBaseTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    protected HttpClient Client => factory.CreateClient();
}
