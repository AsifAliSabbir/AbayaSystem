using System.ComponentModel.DataAnnotations;

namespace AbayaSystem.Core
{
    public enum ExternalWorkerType
    {
        Hybrid = 1,
        FullExternal = 2,
        Both = 3
    }

    public class ExternalWorker
    {
        [Key]
        public int ExternalWorkerId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public ExternalWorkerType SupportedType { get; set; } = ExternalWorkerType.Both;

        public bool IsActive { get; set; } = true;
    }

    public enum ItemCategory
    {
        Abaya = 1,
        InnerDress = 2
    }
}