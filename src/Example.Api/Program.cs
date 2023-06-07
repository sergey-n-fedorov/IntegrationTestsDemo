using Example.Api.Middleware;
using Example.Data;
using Example.Data.Repositories;
using Example.Services;
using Example.Services.External;
using Example.Services.Mappings;
using Example.Shared;
using Example.Shared.Clients;
using Example.Shared.Context;
using Microsoft.EntityFrameworkCore;
using Refit;
using HttpClientFactoryExtensions = Refit.HttpClientFactoryExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<IntegrationContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IExampleContextProvider, ExampleContextProvider>();

builder.Services.AddSingleton(w => new IntegrationContextConfiguration
{
    ConnectionString = w.GetRequiredService<IConfiguration>().GetConnectionString("IntergrationDb")!
});


builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddTransient<CorrelationIdHandler>();

HttpClientFactoryExtensions.AddRefitClient<IExternalServiceClient>(builder.Services)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.external.com"));


WebApplication app = builder.Build();

app.MapControllers();

app.UseMiddleware<CorrelationIdMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<IntegrationContext>();
    context.Database.Migrate();
}

app.Run();

