using Elsa.Persistence.Common.Implementations;
using Elsa.Workflows.Persistence.Entities;
using Elsa.Workflows.Persistence.Services;
using Volte.Data.Dapper;
using Volte.Data.SqlKata;

namespace Elsa.Workflows.Persistence.Implementations;

public class VolteWorkflowBookmarkStore : IWorkflowBookmarkStore
{
    private readonly VolteStore<WorkflowBookmark> _store;
    const string _tableName = "Workflow_Bookmark";

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
        var _criteria = new Query(_tableName);

        _criteria.Where("Id", "=", id);

        var bookmark = _store.Find(_criteria);
        return Task.FromResult(bookmark);
    }

    public Task<IEnumerable<WorkflowBookmark>> FindManyAsync(string name, string? hash, CancellationToken cancellationToken = default)
    {
        var _criteria = new Query(_tableName);

        _criteria.Where("Name", "=", name);

        if (!string.IsNullOrWhiteSpace(hash))
        {
            _criteria.Where("Hash", "=", hash);
        }
        var bookmarks = _store.Query(_criteria);

        return Task.FromResult(bookmarks);
    }

    public Task<IEnumerable<WorkflowBookmark>> FindManyByWorkflowInstanceIdAsync(string workflowInstanceId, CancellationToken cancellationToken = default)
    {
        var _criteria = new Query(_tableName);

        _criteria.Where("WorkflowInstanceId", "=", workflowInstanceId);

        var bookmarks = _store.FindMany(_criteria);
        return Task.FromResult(bookmarks);
    }

    public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var _criteria = new Query(_tableName);
        _criteria.Where("Id", "=", id);

        var result = _store.Delete(_criteria);
        return Task.FromResult(result);
    }

    public Task<int> DeleteManyAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
    {
        var _criteria = new Query(_tableName);

        foreach (string id in ids)
        {
            _criteria.OrWhere("Id", "=", id);
        }

        var result = _store.DeleteMany(_criteria);
        return Task.FromResult(result);
    }
}