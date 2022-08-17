using Elsa.Workflows.VoltePersistence.Entities;

namespace Elsa.Workflows.VoltePersistence.Comparers;

public class WorkflowTriggerHashEqualityComparer : IEqualityComparer<WorkflowTrigger>
{
    public bool Equals(WorkflowTrigger? x, WorkflowTrigger? y) => x?.Hash?.Equals(y?.Hash) ?? false;
    public int GetHashCode(WorkflowTrigger obj) => obj.Hash?.GetHashCode() ?? "".GetHashCode();
}