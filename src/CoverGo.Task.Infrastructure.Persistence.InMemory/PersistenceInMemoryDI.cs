using CoverGo.Task.Application;
using CoverGo.Task.Infrastructure.Persistence.InMemory;

namespace Microsoft.Extensions.DependencyInjection;

public static class PersistenceInMemoryDI
{
    public static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
    {
        services.AddScoped<ICompaniesQuery, InMemoryCompaniesRepository>();
        services.AddScoped<IPlansQuery, InMemoryPlansRepository>();
        services.AddScoped<ICompaniesWriteRepository, InMemoryCompaniesRepository>();
        services.AddScoped<IPlansWriteRepository, InMemoryPlansRepository>();

        var proposalsRepository = new InMemoryProposalsRepository();
        services.AddSingleton<IProposalsQuery>(proposalsRepository);
        services.AddSingleton<IProposalsWriteRepository>(proposalsRepository);
        return services;
    }
}
