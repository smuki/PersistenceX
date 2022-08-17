using Elsa.Features.Services;
using Elsa.Persistence.Common.Entities;
using Elsa.Workflows.VoltePersistence.Features;
using Elsa.Workflows.VoltePersistence.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volte.Data.Dapper;

namespace Elsa.Workflows.VoltePersistence.Extensions;

public static class DependencyInjectionExtensions
{

    public static IModule UseWorkflowVoltePersistence(this IModule module, Action<WorkflowPersistenceFeature>? configure = default)
    {
        module.Configure(configure);
        return module;
    }

    public static WorkflowPersistenceFeature UseWorkflowPersistence(this WorkflowPersistenceFeature feature, Action<WorkflowPersistenceFeature>? configure = default)
    {
        feature.Module.Configure(configure);
        return feature;
    }
   
    public static IServiceCollection AddVolteStore<TEntity, TStore>(this IServiceCollection services) where TEntity : Entity where TStore : class
    {
        services.AddTransient<IDbContext, DbContext>();

        services.TryAddSingleton<VolteStore<TEntity>>();
        services.TryAddSingleton<TStore>();
        return services;
    }

}