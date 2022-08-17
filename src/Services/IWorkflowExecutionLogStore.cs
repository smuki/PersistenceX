using Elsa.Persistence.Common.Models;
using Elsa.Workflows.VoltePersistence.Entities;

namespace Elsa.Workflows.VoltePersistence.Services;

public interface IWorkflowExecutionLogStore
{
    Task SaveAsync(WorkflowExecutionLogRecord record, CancellationToken cancellationToken = default);
    Task SaveManyAsync(IEnumerable<WorkflowExecutionLogRecord> records, CancellationToken cancellationToken = default);
    Task<Page<WorkflowExecutionLogRecord>> FindManyByWorkflowInstanceIdAsync(string workflowInstanceId, PageArgs? pageArgs = default, CancellationToken cancellationToken = default);
}