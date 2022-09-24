using Elsa.Features.Abstractions;
using Elsa.Features.Attributes;
using Elsa.Features.Services;
using Elsa.Common.Extensions;
using Elsa.Workflows.Core.Features;
using Elsa.Workflows.Persistence.Entities;
using Elsa.Workflows.Persistence.Extensions;
//using Elsa.Workflows.Persistence.Implementations;
//using Elsa.Workflows.Persistence.Services;
using Microsoft.Extensions.DependencyInjection;
using Volte.Data.Dapper;
using Elsa.Workflows.Runtime.Features;
using Elsa.Workflows.Management.Features;

namespace Elsa.Workflows.Persistence.Features;

[DependsOn(typeof(WorkflowManagementFeature))]
public class WorkflowPersistenceFeature : PersistenceFeatureBase
{
    public WorkflowPersistenceFeature(IModule module) : base(module)
    {
    }
    
    public override void Configure()
    {
        Module.Configure<WorkflowManagementFeature>(feature =>
        {
            //feature.WorkflowStateStore = sp => sp.GetRequiredService<CoreWorkflowStateStore>();
            //feature.WorkflowTriggerStore = sp => sp.GetRequiredService<EFCoreWorkflowTriggerStore>();
            //feature.BookmarkStore = sp => sp.GetRequiredService<EFCoreBookmarkStore>();
            //feature.WorkflowExecutionLogStore = sp => sp.GetRequiredService<EFCoreWorkflowExecutionLogStore>();
        });
    }
    //public override void Apply() =>
        //Services
        //    .AddTransient<IDbContext, DbContext>()
        //    .AddVolteStore<WorkflowDefinition, VolteWorkflowDefinitionStore>()
        //    .AddVolteStore<WorkflowInstance, VolteWorkflowInstanceStore>()
        //    .AddVolteStore<WorkflowBookmark, VolteWorkflowBookmarkStore>()
        //    .AddVolteStore<WorkflowTrigger, VolteWorkflowTriggerStore>()
        //    .AddVolteStore<WorkflowExecutionLogRecord, VolteWorkflowExecutionLogStore>()
        //    .AddSingleton(WorkflowDefinitionStore)
        //    .AddSingleton(WorkflowInstanceStore)
        //    .AddSingleton(WorkflowBookmarkStore)
        //    .AddSingleton(WorkflowTriggerStore)
        //    .AddSingleton(WorkflowExecutionLogStore);
}