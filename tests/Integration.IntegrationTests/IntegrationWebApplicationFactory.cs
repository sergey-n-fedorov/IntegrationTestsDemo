using Integration.Api.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Integration.IntegrationTests;

public class IntegrationWebApplicationFactory : WebApplicationFactory<UserController>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services => {
            

        });
        return base.CreateHost(builder);
    }
}