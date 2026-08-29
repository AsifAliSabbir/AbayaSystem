namespace AbayaSystem.Core;

public class DashboardSummary
{
    public int TotalOrders { get; set; }
    public int TotalItems { get; set; }
    public int ActiveItems { get; set; }
    public int DeliveredItems { get; set; }
    public int UrgentOrders { get; set; }
    public int OverdueOrders { get; set; }
    public int PendingFabricProcurement { get; set; }
    public int ExternalItemsInProgress { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DepositsReceived { get; set; }
    public decimal BalanceDue { get; set; }
    public List<DashboardStatusCount> StatusCounts { get; set; } = new();
    public List<DashboardRecentOrder> RecentOrders { get; set; } = new();
    public List<DashboardUndeliveredItem> UndeliveredItems { get; set; } = new();
    public List<DashboardWorkerTask> WorkerTasks { get; set; } = new();
}

public class DashboardStatusCount
{
    public ItemStatus Status { get; set; }
    public int Count { get; set; }
}

public class DashboardRecentOrder
{
    public int BranchId { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime EstimatedDeliveryDate { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsUrgent { get; set; }
    public int ItemCount { get; set; }
    public ItemStatus? PrimaryStatus { get; set; }
}

public class DashboardUndeliveredItem
{
    public int BranchId { get; set; }
    public int OrderItemId { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public string ModelDescription { get; set; } = string.Empty;
    public ItemStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime EstimatedDeliveryDate { get; set; }
    public bool IsUrgent { get; set; }
}

public class DashboardWorkerTask
{
    public int WorkerId { get; set; }
    public string WorkerName { get; set; } = string.Empty;
    public string WorkerRole { get; set; } = string.Empty;
    public int OrderItemId { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string ModelDescription { get; set; } = string.Empty;
    public ItemStatus Status { get; set; }
    public DateTime TaskStartedAt { get; set; }
    public DateTime EstimatedDeliveryDate { get; set; }
}
