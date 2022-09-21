using Elsa.Common.Entities;
using Newtonsoft.Json;
using Volte.Data.Dapper;
using Volte.Data.Json;

namespace Elsa.Workflows.Persistence.Entities;

/// <summary>
/// Represents a trigger associated with a workflow definition.
/// </summary>
[Serializable]
[AttributeMapping(TableName = "workflow_trigger", Document = true)]
public class WorkflowTrigger : Entity, IDataObject
{
    public string WorkflowDefinitionId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Hash { get; set; }
    public string? Data { get; set; }
    //----------------------------------
    //----------------------------------
    [AttributeMapping(Indexes = true, PrimaryKey = true)]
    public string Id { get; set; }

    [AttributeMapping(Indexes = true)]
    public string sCorporation { get; set; }

    [AttributeMapping(Ignore = true, ColumnName = "$Content")]
    [JsonIgnore]
    public string Content { get; set; }

    [AttributeMapping(Ignore = true)]
    [JsonIgnore]
    public DataState State { get; set; }
    public int Version { get; set; }

}