using Integration.Shared.Models;
using Refit;

namespace Integration.Shared.Clients;

public interface IIntegrationServiceClient
{
    [Get("/api/v1/User")]
    Task<List<User>> GetAlertsAsync();
}