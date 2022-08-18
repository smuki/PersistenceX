using Elsa.Persistence.Common.Entities;
using Elsa.Persistence.Common.Implementations;
using Elsa.Persistence.Common.Models;
using Elsa.Workflows.Persistence.Entities;
using Elsa.Workflows.Persistence.Models;
using Elsa.Workflows.Persistence.Services;
using Volte.Data.Dapper;
using Volte.Data.SqlKata;

namespace Elsa.Workflows.Persistence.Implementations;

public class VolteWorkflowInstanceStore : IWorkflowInstanceStore
{
    private readonly VolteStore<WorkflowInstance> _store;
    const string _tableName = "Workflow_Instance";
    public VolteWorkflowInstanceStore(VolteStore<WorkflowInstance> store)
    {
        _store = store;
    }

    public Task<WorkflowInstance?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var _criteria = new Query(_tableName);
        _criteria.Where("Id", "=", id);

        var instance = _store.Find(_criteria);

        return Task.FromResult(instance);
    }

    public Task SaveAsync(WorkflowInstance record, CancellationToken cancellationToken = default)
    {
         _store.Save(record);
        return Task.CompletedTask;
    }

    public Task SaveManyAsync(IEnumerable<WorkflowInstance> records, CancellationToken cancellationToken = default)
    {
        _store.SaveMany(records);
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var _criteria = QueryBuilder<WorkflowInstance>.Builder(_store.Trans);
        _criteria.Where("Id", Operation.Equal, id);

        var success = _store.Delete(_criteria);
        return Task.FromResult(success);
    }

    public Task<int> DeleteManyAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
    {
        var _criteria = new Query(_tableName);

        foreach (string id in ids)
        {
            _criteria.OrWhere("Id", "=", id);
        }

        var count = _store.DeleteMany(_criteria);
        return Task.FromResult(count);
    }

    public Task DeleteManyByDefinitionIdAsync(string definitionId, CancellationToken cancellationToken = default)
    {
        var _criteria = new Query(_tableName);

        _criteria.Where("DefinitionId","=", definitionId);

        _store.DeleteWhere(_criteria);
        return Task.CompletedTask;
    }

    public Task<Page<WorkflowInstanceSummary>> FindManyAsync(FindWorkflowInstancesArgs args, CancellationToken cancellationToken = default)
    {
        var query = _store.List().AsQueryable();
        var (searchTerm, definitionId, version, correlationId, workflowStatus, workflowSubStatus, pageArgs, orderBy, orderDirection) = args;

        if (!string.IsNullOrWhiteSpace(definitionId))
            query = query.Where(x => x.DefinitionId == definitionId);

        if (version != null)
            query = query.Where(x => x.Version == version);

        if (!string.IsNullOrWhiteSpace(correlationId))
            query = query.Where(x => x.CorrelationId == correlationId);

        if (workflowStatus != null)
            query = query.Where(x => x.Status == workflowStatus);
        
        if (workflowSubStatus != null)
            query = query.Where(x => x.SubStatus == workflowSubStatus);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            const StringComparison comparison = StringComparison.InvariantCultureIgnoreCase;

            query =
                from instance in query
                where instance.Name != null && instance.Name.Contains(searchTerm, comparison) == true
                      || instance.Id.Contains(searchTerm, comparison)
                      || instance.DefinitionId.Contains(searchTerm, comparison)
                      || instance.CorrelationId != null && instance.CorrelationId.Contains(searchTerm, comparison)
                select instance;
        }

        //query = orderBy switch
        //{
        //    OrderBy.Finished => orderDirection == OrderDirection.Ascending ? query.OrderBy(x => x.FinishedAt) : query.OrderByDescending(x => x.FinishedAt),
        //    OrderBy.LastExecuted => orderDirection == OrderDirection.Ascending ? query.OrderBy(x => x.LastExecutedAt) : query.OrderByDescending(x => x.LastExecutedAt),
        //    OrderBy.Created => orderDirection == OrderDirection.Ascending ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
        //    _ => query
        //};

        var totalCount = query.Count();

        if (pageArgs?.Offset != null) query = query.Skip(pageArgs.Offset.Value);
        if (pageArgs?.Limit != null) query = query.Take(pageArgs.Limit.Value);
        
        var entities = query.ToList();
        var summaries = entities.Select(WorkflowInstanceSummary.FromInstance).ToList();
        var pagedList = Page.Of(summaries, totalCount);
        return Task.FromResult(pagedList);
    }
}