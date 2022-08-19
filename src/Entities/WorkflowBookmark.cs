using Elsa.Persistence.Common.Entities;
using Elsa.Workflows.Core.Models;
using Newtonsoft.Json;
using Volte.Data.Dapper;
using Volte.Data.Json;

namespace Elsa.Workflows.Persistence.Entities;

[Serializable]
[AttributeMapping(TableName = "workflow_bookmark",Document =true)]
public class WorkflowBookmark : Entity, IDataObject
{
    public string Name { get; init; } = default!;
    public string? Hash { get; set; }
    public string? Data { get; set; }
    public string WorkflowDefinitionId { get; init; } = default!;
    public string WorkflowInstanceId { get; init; } = default!;
    public string? CorrelationId { get; init; }
    public string ActivityId { get; init; } = default!;
    public string ActivityInstanceId { get; init; } = default!;
    public string? CallbackMethodName { get; set; }

    public Bookmark ToBookmark() => new(Id, Name, Hash, Data, ActivityId, ActivityInstanceId, CallbackMethodName);

    public static WorkflowBookmark FromBookmark(Bookmark bookmark, WorkflowInstance workflowInstance) =>
        new()
        {
            Id = bookmark.Id,
            WorkflowDefinitionId = workflowInstance.DefinitionId,
            WorkflowInstanceId = workflowInstance.Id,
            CorrelationId = workflowInstance.CorrelationId,
            Hash = bookmark.Hash,
            Data = bookmark.Data,
            Name = bookmark.Name,
            ActivityId = bookmark.ActivityId,
            ActivityInstanceId = bookmark.ActivityInstanceId,
            CallbackMethodName = bookmark.CallbackMethodName
        };

    //----------------------------------
    //----------------------------------
    [AttributeMapping(Indexes = true, PrimaryKey = true)]
    public string Id { get; set; }

    [AttributeMapping(Indexes = true)]
    public string sCorporation { get; set; }

    [AttributeMapping(Ignore = true,ColumnName = "$Content")]
    [JsonIgnore]
    public string Content { get; set; }

    [AttributeMapping(Ignore = true)]
    [JsonIgnore]
    public DataState State { get; set; }
    public int Version { get; set; }

}