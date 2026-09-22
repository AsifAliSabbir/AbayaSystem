namespace AbayaSystem.Core;

public class WorkerPerformanceReport
{
    public Worker Worker { get; set; } = new();
    public int TotalWorkflowEvents { get; set; }
    public int AssignedItems { get; set; }
    public int CompletedItems { get; set; }
    public int ActiveItems { get; set; }
    public DateTime? LastActivity { get; set; }
    public List<OrderWorkflowEventDto> WorkflowHistory { get; set; } = new();
}
