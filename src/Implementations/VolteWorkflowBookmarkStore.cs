using Elsa.Persistence.Common.Implementations;
using Elsa.Workflows.Persistence.Entities;
using Elsa.Workflows.Persistence.Services;
using Volte.Data.Dapper;

namespace Elsa.Workflows.Persistence.Implementations;

public class VolteWorkflowBookmarkStore : IWorkflowBookmarkStore
{
    private readonly VolteStore<WorkflowBookmark> _store;

    public VolteWorkflowBookmarkStore(VolteStore<WorkflowBookmark> store)
    {
        _store = store;
    }
    
    public Task SaveAsync(WorkflowBookmark record, CancellationToken cancellationToken = default)
    {
        _store.Save(record);
        return Task.CompletedTask;
    }

    public Task SaveManyAsync(IEnumerable<WorkflowBookmark> records, CancellationToken cancellationToken = default)
    {
        _store.SaveMany(records);
        return Task.CompletedTask;
    }

    public Task<WorkflowBookmark?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var _criteria = QueryBuilder<WorkflowBookmark>.Builder(_store.Trans);
        _criteria.Where("Id", Operation.Equal, id);

        var bookmark = _store.Find(_criteria);
        return Task.FromResult(bookmark);
    }

    public Task<IEnumerable<WorkflowBookmark>> FindManyAsync(string name, string? hash, CancellationToken cancellationToken = default)
    {
        var _criteria = QueryBuilder<WorkflowBookmark>.Builder(_store.Trans);
        _criteria.Where("Name", Operation.Equal, name);

        if (!string.IsNullOrWhiteSpace(hash))
        {
            _criteria.Where("Hash", Operation.Equal, hash);
        }
        var bookmarks = _store.Query(_criteria);

        return Task.FromResult(bookmarks);
    }

    public Task<IEnumerable<WorkflowBookmark>> FindManyByWorkflowInstanceIdAsync(string workflowInstanceId, CancellationToken cancellationToken = default)
    {
        var _criteria = QueryBuilder<WorkflowBookmark>.Builder(_store.Trans);
        _criteria.Where("WorkflowInstanceId", Operation.Equal, workflowInstanceId);

        var bookmarks = _store.FindMany(_criteria);
        return Task.FromResult(bookmarks);
    }

    public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var _criteria = QueryBuilder<WorkflowBookmark>.Builder(_store.Trans);
        _criteria.Where("Id", Operation.Equal, id);

        var result = _store.Delete(_criteria);
        return Task.FromResult(result);
    }

    public Task<int> DeleteManyAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
    {

        var _criteria = QueryBuilder<WorkflowBookmark>.Builder(_store.Trans);
        foreach (string id in ids)
        {
            _criteria.OrWhereClause("Id", Operation.Equal, id);
        }

        var result = _store.DeleteMany(_criteria);
        return Task.FromResult(result);
    }
}