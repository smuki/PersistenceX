using Elsa.Workflows.Management.Features;

namespace Elsa.Persistence.EntityFrameworkCore.Modules.Management
{
    public static class Extensions
    {
        public static WorkflowManagementFeature UseWorkflowsPersistence(this WorkflowManagementFeature feature, Action<EFCoreManagementPersistenceFeature>? configure = default)
        {
            feature.Module.Configure(configure);
            return feature;
        }
    }
}