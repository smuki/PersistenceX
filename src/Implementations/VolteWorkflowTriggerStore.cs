using Elsa.Persistence.Common.Implementations;
using Elsa.Workflows.Persistence.Entities;
using Elsa.Workflows.Persistence.Services;
using Volte.Data.Dapper;
using Volte.Data.SqlKata;

namespace Elsa.Workflows.Persistence.Implementations;

public class VolteWorkflowTriggerStore : IWorkflowTriggerStore
{
    private readonly VolteStore<WorkflowTrigger> _store;
    const string _tableName = "Workflow_Trigger";

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
        var _criteria = new Query(_tableName);

        _criteria.Where("Name", "=", name);
        if (!string.IsNullOrWhiteSpace(hash))
        {
            _criteria.Where("Hash", "=", hash);
        }

        var triggers = _store.Query(_criteria);

        return Task.FromResult<IEnumerable<WorkflowTrigger>>(triggers.ToList());
    }

    public Task<IEnumerable<WorkflowTrigger>> FindManyByWorkflowDefinitionIdAsync(string workflowDefinitionId, CancellationToken cancellationToken = default)
    {
        var _criteria = new Query(_tableName);
        _criteria.Where("WorkflowDefinitionId", "=", workflowDefinitionId);

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
        var _criteria = new Query(_tableName);
        foreach (string id in ids)
        {
            _criteria.OrWhere("Id", "=", id);
        }

        _store.DeleteMany(_criteria);
        return Task.CompletedTask;
    }
}