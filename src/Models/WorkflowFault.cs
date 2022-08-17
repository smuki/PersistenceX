﻿namespace Elsa.Workflows.VoltePersistence.Models
{
    public record WorkflowFault(SimpleException? Exception, string Message, string? FaultedActivityId, object? ActivityInput, bool Resuming);
}