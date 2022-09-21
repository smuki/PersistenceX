using Elsa.Common.Extensions;
using Elsa.Common.Implementations;
using Elsa.Common.Models;
using Elsa.Workflows.Persistence.Entities;
using Elsa.Workflows.Persistence.Services;
using Volte.Data.Dapper;
using Volte.Data.SqlKata;

namespace Elsa.Workflows.Persistence.Implementations;

public class VolteWorkflowExecutionLogStore : IWorkflowExecutionLogStore
{
    private readonly VolteStore<WorkflowExecutionLogRecord> _store;
    const string _tableName = "Workflow_ExecutionLog_Record";

    public VolteWorkflowExecutionLogStore(VolteStore<WorkflowExecutionLogRecord> store)
    {
        _store = store;
    }
    
    public Task SaveAsync(WorkflowExecutionLogRecord record, CancellationToken cancellationToken = default)
    {
        _store.Save(record);
        return Task.CompletedTask;
    }

    public Task SaveManyAsync(IEnumerable<WorkflowExecutionLogRecord> records, CancellationToken cancellationToken = default)
    {
        _store.SaveMany(records);
        return Task.CompletedTask;
    }

    public Task<Page<WorkflowExecutionLogRecord>> FindManyByWorkflowInstanceIdAsync(string workflowInstanceId, PageArgs? pageArgs = default, CancellationToken cancellationToken = default)
    {
        var _criteria = new Query(_tableName);

        _criteria.Where("WorkflowInstanceId", "=", workflowInstanceId);
        _criteria.OrderBy("Timestamp");

        var page = _store
            .FindMany(_criteria)
            .Paginate();

        return Task.FromResult(page);
    }
}