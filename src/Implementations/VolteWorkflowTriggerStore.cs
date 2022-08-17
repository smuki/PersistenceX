using Elsa.Persistence.Common.Implementations;
using Elsa.Workflows.VoltePersistence.Entities;
using Elsa.Workflows.VoltePersistence.Services;
using Volte.Data.Dapper;

namespace Elsa.Workflows.VoltePersistence.Implementations;

public class VolteWorkflowTriggerStore : IWorkflowTriggerStore
{
    private readonly VolteStore<WorkflowTrigger> _store;

    public VolteWorkflowTriggerStore(VolteStore<WorkflowTrigger> store)
    {
        _store = store;
    }

    public Task SaveAsync(WorkflowTrigger record, CancellationToken cancellationToken = default)
    {
        _store.Save(record);
        return Task.CompletedTask;
    }

    public Task SaveManyAsync(IEnumerable<WorkflowTrigger> records, CancellationToken cancellationToken = default)
    {
        _store.SaveMany(records);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<WorkflowTrigger>> FindManyByNameAsync(string name, string? hash, CancellationToken cancellationToken = default)
    {

        var _criteria = QueryBuilder<WorkflowTrigger>.Builder(_store.Trans);
        _criteria.Where("Name", Operation.Equal, name);
        if (!string.IsNullOrWhiteSpace(hash))
        {
            _criteria.Where("Hash", Operation.Equal, hash);
        }

        var triggers = _store.Query(_criteria);

        return Task.FromResult<IEnumerable<WorkflowTrigger>>(triggers.ToList());
    }

    public Task<IEnumerable<WorkflowTrigger>> FindManyByWorkflowDefinitionIdAsync(string workflowDefinitionId, CancellationToken cancellationToken = default)
    {
        var _criteria = QueryBuilder<WorkflowTrigger>.Builder(_store.Trans);
        _criteria.Where("WorkflowDefinitionId", Operation.Equal, workflowDefinitionId);
        var triggers = _store.Query(_criteria);
        return Task.FromResult<IEnumerable<WorkflowTrigger>>(triggers.ToList());
    }

    public Task ReplaceAsync(IEnumerable<WorkflowTrigger> removed, IEnumerable<WorkflowTrigger> added, CancellationToken cancellationToken = default)
    {
        _store.DeleteMany(removed);
        _store.SaveMany(added);

        return Task.CompletedTask;
    }

    public Task DeleteManyAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
    {
        var _criteria = QueryBuilder<WorkflowTrigger>.Builder(_store.Trans);
        foreach (string id in ids)
        {
            _criteria.OrWhereClause("Id", Operation.Equal, id);
        }

        _store.DeleteMany(_criteria);
        return Task.CompletedTask;
    }
}