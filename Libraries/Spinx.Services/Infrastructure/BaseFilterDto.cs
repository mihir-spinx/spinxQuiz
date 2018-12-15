using System;
using System.Collections.Generic;

namespace Spinx.Services.Infrastructure
{
    public abstract class BaseFilterDto
    {
        public string Action { get; set; }
        public List<int> Ids { get; set; }

        public int Page { get; set; }
        public int Size { get; set; }

        public string SortColumn { get; set; }
        public string SortType { get; set; }

        public DateTime? FromCreatedAt { get; set; }
        public DateTime? ToCreatedAt { get; set; }

        public DateTime? FromExpiryDate { get; set; }
        public DateTime? ToExpiryDate { get; set; }

        public DateTime? FromUpdatedAt { get; set; }
        public DateTime? ToUpdatedAt { get; set; }
    }
}