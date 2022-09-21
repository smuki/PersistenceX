using Elsa.Common.Entities;
using Elsa.Workflows.Core.Models;
using Elsa.Workflows.Core.State;
using Elsa.Workflows.Persistence.Models;
using Newtonsoft.Json;
using Volte.Data.Dapper;
using Volte.Data.Json;

namespace Elsa.Workflows.Persistence.Entities;

[Serializable]
[AttributeMapping(TableName = "workflow_instance", Document = true)]
public class WorkflowInstance : Entity, IDataObject
{
    public string DefinitionId { get; init; } = default!;
    public string DefinitionVersionId { get; init; } = default!;
    public int Version { get; set; }
    public WorkflowState WorkflowState { get; set; } = default!;
    public WorkflowStatus Status { get; set; }

    public WorkflowSubStatus SubStatus { get; set; }
    public string? CorrelationId { get; set; }
    public string? Name { get; set; }
    public WorkflowFault? Fault { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? LastExecutedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }
    public DateTimeOffset? CancelledAt { get; set; }
    public DateTimeOffset? FaultedAt { get; set; }

    //----------------------------------
    //----------------------------------
    [AttributeMapping(Indexes = true, PrimaryKey = true)]
    public  string Id { get; set; }

    [AttributeMapping(Indexes = true)]
    public string sCorporation { get; set; }

    [AttributeMapping(Ignore = true, ColumnName = "$Content")]
    [JsonIgnore]
    public string Content { get; set; }

    [AttributeMapping(Ignore = true)]
    [JsonIgnore]
    public DataState State { get; set; }
}