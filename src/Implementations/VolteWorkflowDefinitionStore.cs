using Elsa.Common.Extensions;
using Elsa.Common.Implementations;
using Elsa.Common.Models;
using Elsa.Workflows.Persistence.Entities;
using Elsa.Workflows.Persistence.Extensions;
using Elsa.Workflows.Persistence.Models;
using Elsa.Workflows.Persistence.Services;
using Volte.Data.Dapper;
using Volte.Data.SqlKata;

namespace Elsa.Workflows.Persistence.Implementations;

public class VolteWorkflowDefinitionStore : IWorkflowDefinitionStore
{
    private readonly VolteStore<WorkflowDefinition> _store;
    private readonly VolteStore<WorkflowInstance> _instanceStore;
    private readonly VolteStore<WorkflowTrigger> _triggerStore;
    private readonly VolteStore<WorkflowBookmark> _bookmarkStore;
    private Dictionary<string, object> where = new();
    const string _tableName = "Workflow_Definition";

    public VolteWorkflowDefinitionStore(
        VolteStore<WorkflowDefinition> store,
        VolteStore<WorkflowInstance> instanceStore,
        VolteStore<WorkflowTrigger> triggerStore,
        VolteStore<WorkflowBookmark> bookmarkStore)
    {
        _store = store;
        _instanceStore = instanceStore;
        _triggerStore = triggerStore;
        _bookmarkStore = bookmarkStore;
    }

    public Task<WorkflowDefinition?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var _criteria = new Query(_tableName);

        _criteria.Where("Id", "=", id);

        var definition = _store.Find(_criteria);
        return Task.FromResult(definition);
    }
    //public KeyValuePair versionOptions(VersionOptions versionOptions)
    //{
    //    //if (versionOptions.IsDraft)
    //    //    return predicate.And(x => !x.IsPublished);
    //    //if (versionOptions.IsLatest)
    //    //    return predicate.And(x => x.IsLatest);
    //    //if (versionOptions.IsPublished)
    //    //    return predicate.And(x => x.IsPublished);
    //    //if (versionOptions.IsLatestOrPublished)
    //    //    return predicate.And(x => x.IsPublished || x.IsLatest);
    //    //if (versionOptions.Version > 0)
    //    //    return predicate.And(x => x.Version == versionOptions.Version);
    //    return null;
    //}
    public Task<WorkflowDefinition?> FindByDefinitionIdAsync(string definitionId, VersionOptions versionOptions, CancellationToken cancellationToken = default)
    {
        var _criteria = new Query(_tableName);

        //var _criteria = QueryBuilder<WorkflowDefinition>.Builder(_store.Trans);
        _criteria.Where("DefinitionId", "=", definitionId);
        //var definition = _store.Find(x => x.DefinitionId == definitionId && x.WithVersion(versionOptions));
        var definition = _store.Find(_criteria);

        return Task.FromResult(definition);
    }

    public Task<WorkflowDefinition?> FindByNameAsync(string name, VersionOptions versionOptions, CancellationToken cancellationToken = default)
    {
        var _criteria = new Query(_tableName);

        //var _criteria = QueryBuilder<WorkflowDefinition>.Builder(_store.Trans);
        _criteria.Where("Name", "=", name);
        //var definition = _store.Find(x => x.Name == name && x.WithVersion(versionOptions));
        var definition = _store.Find(_criteria);

        return Task.FromResult(definition);
    }

    public Task<IEnumerable<WorkflowDefinitionSummary>> FindManySummariesAsync(IEnumerable<string> definitionIds, VersionOptions? versionOptions = default, CancellationToken cancellationToken = default)
    {
        var query = _store.List();

        if (versionOptions != null)
            query = query.WithVersion(versionOptions.Value);

        var definitionIdList = definitionIds.ToList();
        query = query.Where(x => definitionIdList.Contains(x.Id));

        var summaries = query.Select(WorkflowDefinitionSummary.FromDefinition).ToList().AsEnumerable();
        return Task.FromResult(summaries);
    }

    public Task<IEnumerable<WorkflowDefinition>> FindLatestAndPublishedByDefinitionIdAsync(string definitionId, CancellationToken cancellationToken = default)
    {
        var _criteria = new Query(_tableName);

        //var _criteria = QueryBuilder<WorkflowDefinition>.Builder(_store.Trans);
        _criteria.Where("DefinitionId", "=", definitionId);
        //var definitions = _store.FindMany(x => x.DefinitionId == definitionId && (x.IsLatest || x.IsPublished));
        var definitions = _store.FindMany(_criteria);
        return Task.FromResult(definitions);
    }

    public Task SaveAsync(WorkflowDefinition record, CancellationToken cancellationToken = default)
    {
        _store.Save(record);
        return Task.CompletedTask;
    }

    public Task SaveManyAsync(IEnumerable<WorkflowDefinition> records, CancellationToken cancellationToken = default)
    {
        _store.SaveMany(records);
        return Task.CompletedTask;
    }

    public Task<int> DeleteByDefinitionIdAsync(string definitionId, CancellationToken cancellationToken = default)
    {
        var _criteria = new Query(_tableName);

        _criteria.Where("WorkflowDefinitionId", "=", definitionId);

        _triggerStore.DeleteWhere(_criteria);
        _bookmarkStore.DeleteWhere(_criteria);

        var _criteria2 = new Query(_tableName);
        _criteria2.Where("DefinitionId", "=", definitionId);

        _instanceStore.DeleteWhere(_criteria2);
        var result = _store.DeleteWhere(_criteria2);
        return Task.FromResult(result);
    }

    public Task<int> DeleteByDefinitionIdsAsync(IEnumerable<string> definitionIds, CancellationToken cancellationToken = default)
    {
        var definitionIdList = definitionIds.ToList();

        var _criteria = new Query(_tableName);
        foreach (string id in definitionIdList)
        {
            _criteria.OrWhere("WorkflowDefinitionId", "=", id);
        }

        _triggerStore.DeleteWhere(_criteria);
        _bookmarkStore.DeleteWhere(_criteria);

        var _criteria2 = new Query(_tableName);
        foreach (string id in definitionIdList)
        {
            _criteria2.OrWhere("DefinitionId", "=", id);
        }

        _instanceStore.DeleteWhere(_criteria2);
        var result = _store.DeleteWhere(_criteria2);
        return Task.FromResult(result);
    }

    public Task<Page<WorkflowDefinitionSummary>> ListSummariesAsync(
        VersionOptions? versionOptions = default,
        string? materializerName = default,
        PageArgs? pageArgs = default,
        CancellationToken cancellationToken = default)
    {
        var query = _store.List().AsQueryable();

        if (versionOptions != null) query = query.WithVersion(versionOptions.Value);
        if (!string.IsNullOrWhiteSpace(materializerName)) query = query.Where(x => x.MaterializerName == materializerName);

        var page = query.Paginate(x => WorkflowDefinitionSummary.FromDefinition(x), pageArgs);
        return Task.FromResult(page);
    }

    public Task<bool> GetExistsAsync(string definitionId, VersionOptions versionOptions, CancellationToken cancellationToken = default)
    {
        var exists = _store.AnyAsync(x => x.DefinitionId == definitionId && x.WithVersion(versionOptions));
        return Task.FromResult(exists);
    }

    private IQueryable<WorkflowDefinition> FilterByLabels(IQueryable<WorkflowDefinition> query, IEnumerable<string>? labelNames)
    {
        // var labelList = labelNames?.Select(x => x.ToLowerInvariant()).ToList();
        //
        // // Do we need to filter by labels?
        // if (labelList == null || !labelList.Any())
        //     return query;
        //
        // // Translate label names to label IDs.
        // var labelIds = _labelStore.FindMany(x => labelList.Contains(x.NormalizedName)).Select(x => x.Id);
        //
        // // We need to build a query that requires a workflow definition to be associated with ALL labels ("and").
        // foreach (var labelId in labelIds)
        //     query =
        //         from workflowDefinition in query
        //         join label in _workflowDefinitionLabelStore.List().AsQueryable()
        //             on workflowDefinition.Id equals label.WorkflowDefinitionVersionId
        //         where labelId == label.LabelId
        //         select workflowDefinition;

        return query;
    }
}