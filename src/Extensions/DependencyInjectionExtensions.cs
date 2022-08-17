using Elsa.Features.Services;
using Elsa.Persistence.Common.Entities;
using Elsa.Workflows.Persistence.Features;
using Elsa.Workflows.Persistence.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Elsa.Workflows.Persistence.Extensions;

public static class DependencyInjectionExtensions
{
    public static IModule UseWorkflowVoltePersistence(this IModule module, Action<WorkflowPersistenceFeature>? configure = default )
    {
        module.Configure(configure);
        return module;
    }
    public static IServiceCollection AddVolteStore<TEntity, TStore>(this IServiceCollection services) where TEntity : Entity where TStore : class
    {
        services.TryAddSingleton<VolteStore<TEntity>>();
        services.TryAddSingleton<TStore>();
        return services;
    }

}