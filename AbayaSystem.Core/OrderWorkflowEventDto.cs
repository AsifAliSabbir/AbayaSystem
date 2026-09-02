namespace AbayaSystem.Core;

public class OrderWorkflowEventDto
{
    public int StatusLogId { get; set; }
    public ItemStatus? PreviousState { get; set; }
    public ItemStatus CurrentState { get; set; }
    public int? PreviousWorkerId { get; set; }
    public int? CurrentWorkerId { get; set; }
    public string PreviousWorkerName { get; set; } = string.Empty;
    public string CurrentWorkerName { get; set; } = string.Empty;
    public DateTime TimeOfEvent { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string EventType { get; set; } = "Workflow Status Change";
}
