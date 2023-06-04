using Integration.Data.Repositories;
using Integration.Services.External;
using Integration.Services.Mappings;
using Integration.Shared.Clients;
using IntegrationService.Data;
using Microsoft.EntityFrameworkCore;
using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<IntegrationContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services
    .AddRefitClient<IExternalServiceClient>()
    .ConfigureHttpClient((provider, client) => { client.BaseAddress = new Uri(baseUrlProviderFunc(provider));})
    .AddHttpMessageHandler<ExampleDelegatingHandler>();


WebApplication app = builder.Build();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<IntegrationContext>();
    context.Database.Migrate();
}

app.Run();

