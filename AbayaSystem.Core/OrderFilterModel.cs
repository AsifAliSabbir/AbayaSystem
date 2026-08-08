using System;
using System.Collections.Generic;

namespace AbayaSystem.Core
{
    public class OrderFilterModel
    {

        public int? BranchId { get; set; } 
        public string? OrderId { get; set; }
        public DateTime? OrderDateFrom { get; set; }
        public DateTime? OrderDateTo { get; set; }
        public DateTime? DeliveryDateFrom { get; set; }
        public DateTime? DeliveryDateTo { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public ItemStatus? ItemStatus { get; set; }
        public string SortBy { get; set; } = "OrderDate"; // "OrderDate" or "DeliveryDate"
        public bool SortDescending { get; set; } = true;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }
}

