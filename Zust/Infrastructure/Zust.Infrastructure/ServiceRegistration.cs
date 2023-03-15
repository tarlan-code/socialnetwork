using Microsoft.Extensions.DependencyInjection;
using Zust.Application.Abstractions.Services;
using Zust.Infrastructure.Services;

namespace Zust.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection service)
        {
            service.AddScoped<IMailService, MailService>();
        }
    }
}
