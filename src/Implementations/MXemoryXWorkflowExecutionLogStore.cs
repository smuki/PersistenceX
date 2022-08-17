using Elsa.Persistence.Common.Extensions;
using Elsa.Persistence.Common.Implementations;
using Elsa.Persistence.Common.Models;
using Elsa.Workflows.Persistence.Entities;
using Elsa.Workflows.Persistence.Services;
using Volte.Data.Dapper;

namespace Elsa.Workflows.Persistence.Implementations;

public class MXemoryXWorkflowExecutionLogStore : IWorkflowExecutionLogStore
{
    private readonly MXemoryXStore<WorkflowExecutionLogRecord> _store;

    public MXemoryXWorkflowExecutionLogStore(MXemoryXStore<WorkflowExecutionLogRecord> store)
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

        var _criteria = QueryBuilder<WorkflowExecutionLogRecord>.Builder(_store.Trans);
            _criteria.Where("WorkflowInstanceId", Operation.Equal, workflowInstanceId);
        _criteria.OrderBy("Timestamp");

        var page = _store
            .FindMany(_criteria)
            .Paginate();
        
        return Task.FromResult(page);
    }
}