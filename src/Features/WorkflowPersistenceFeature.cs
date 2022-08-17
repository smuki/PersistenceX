using Elsa.Features.Abstractions;
using Elsa.Features.Attributes;
using Elsa.Features.Services;
using Elsa.Persistence.Common.Extensions;
using Elsa.Workflows.Core.Features;
using Elsa.Workflows.Persistence.Entities;
using Elsa.Workflows.Persistence.Extensions;
using Elsa.Workflows.Persistence.Implementations;
using Elsa.Workflows.Persistence.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Workflows.Persistence.Features;

[DependsOn(typeof(WorkflowsFeature))]
public class WorkflowPersistenceFeature : FeatureBase
{
    public WorkflowPersistenceFeature(IModule module) : base(module)
    {
    }
    
    public Func<IServiceProvider, IWorkflowDefinitionStore> WorkflowDefinitionStore { get; set; } = sp => sp.GetRequiredService<VolteWorkflowDefinitionStore>();
    public Func<IServiceProvider, IWorkflowInstanceStore> WorkflowInstanceStore { get; set; } = sp => sp.GetRequiredService<VolteWorkflowInstanceStore>();
    public Func<IServiceProvider, IWorkflowBookmarkStore> WorkflowBookmarkStore { get; set; } = sp => sp.GetRequiredService<VolteWorkflowBookmarkStore>();
    public Func<IServiceProvider, IWorkflowTriggerStore> WorkflowTriggerStore { get; set; } = sp => sp.GetRequiredService<VolteWorkflowTriggerStore>();
    public Func<IServiceProvider, IWorkflowExecutionLogStore> WorkflowExecutionLogStore { get; set; } = sp => sp.GetRequiredService<VolteWorkflowExecutionLogStore>();

    public WorkflowPersistenceFeature WithWorkflowDefinitionStore(Func<IServiceProvider, IWorkflowDefinitionStore> factory)
    {
        WorkflowDefinitionStore = factory;
        return this;
    }

    public WorkflowPersistenceFeature WithWorkflowInstanceStore(Func<IServiceProvider, IWorkflowInstanceStore> factory)
    {
        WorkflowInstanceStore = factory;
        return this;
    }

    public WorkflowPersistenceFeature WithWorkflowBookmarkStore(Func<IServiceProvider, IWorkflowBookmarkStore> factory)
    {
        WorkflowBookmarkStore = factory;
        return this;
    }

    public WorkflowPersistenceFeature WithWorkflowTriggerStore(Func<IServiceProvider, IWorkflowTriggerStore> factory)
    {
        WorkflowTriggerStore = factory;
        return this;
    }

    public WorkflowPersistenceFeature WithWorkflowExecutionLogStore(Func<IServiceProvider, IWorkflowExecutionLogStore> factory)
    {
        WorkflowExecutionLogStore = factory;
        return this;
    }

    public override void Apply() =>
        Services
            .AddVolteStore<WorkflowDefinition, VolteWorkflowDefinitionStore>()
            .AddVolteStore<WorkflowInstance, VolteWorkflowInstanceStore>()
            .AddVolteStore<WorkflowBookmark, VolteWorkflowBookmarkStore>()
            .AddVolteStore<WorkflowTrigger, VolteWorkflowTriggerStore>()
            .AddVolteStore<WorkflowExecutionLogRecord, VolteWorkflowExecutionLogStore>()
            .AddSingleton(WorkflowDefinitionStore)
            .AddSingleton(WorkflowInstanceStore)
            .AddSingleton(WorkflowInstanceStore)
            .AddSingleton(WorkflowBookmarkStore)
            .AddSingleton(WorkflowTriggerStore)
            .AddSingleton(WorkflowExecutionLogStore);
}