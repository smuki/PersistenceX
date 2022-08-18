using Elsa.Persistence.Common.Entities;
using Newtonsoft.Json;
using Volte.Data.Dapper;
using Volte.Data.Json;

namespace Elsa.Workflows.Persistence.Entities
{
    public class WorkflowExecutionLogRecord : Entity, IDataObject
    {
        public string WorkflowDefinitionId { get; set; } = default!;
        public string WorkflowInstanceId { get; set; } = default!;
        public string ActivityInstanceId { get; set; } = default!;
        public string? ParentActivityInstanceId { get; set; }
        public string ActivityId { get; set; } = default!;
        public string ActivityType { get; set; } = default!;
        public DateTimeOffset Timestamp { get; set; }
        public string? EventName { get; set; }
        public string? Message { get; set; }
        public string? Source { get; set; }
        public object? Payload { get; set; }

        //----------------------------------
        //----------------------------------
        [AttributeMapping(Indexes = true, PrimaryKey = true)]
        public string Id { get; set; }

        [AttributeMapping(Indexes = true)]
        public string sCorporation { get; set; }

        [AttributeMapping(Ignore = true)]
        [JsonIgnore]
        public string Content { get; set; }

        [AttributeMapping(Ignore = true)]
        [JsonIgnore]
        public DataState State { get; set; }
        public int Version { get; set; }

    }
}