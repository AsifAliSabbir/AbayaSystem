using System.ComponentModel.DataAnnotations;

namespace AbayaSystem.Core
{
    public enum ExternalVendorJobStage
    {
        RawFabricEmbroidery = 1,
        HalfStitchEmbroidery = 2,
        FullExternalProduction = 3
    }

    public enum ExternalVendorJobStatus
    {
        Dispatched = 1,
        Returned = 2,
        Cancelled = 3
    }

    public class ExternalVendorJob
    {
        [Key]
        public int ExternalVendorJobId { get; set; }

        public int OrderItemId { get; set; }
        public int BranchId { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public OrderItem? OrderItem { get; set; }

        public int ExternalWorkerId { get; set; }
        public ExternalWorker? ExternalWorker { get; set; }

        public ExternalVendorJobStage Stage { get; set; }
        public ExternalVendorJobStatus Status { get; set; } = ExternalVendorJobStatus.Dispatched;

        public DateTime DispatchedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpectedReturnDate { get; set; }
        public DateTime? ReturnedAt { get; set; }

        [MaxLength(1000)]
        public string DispatchNotes { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ReturnNotes { get; set; } = string.Empty;

        public int? DispatchedByWorkerId { get; set; }
        public int? ReceivedByWorkerId { get; set; }
    }
}
