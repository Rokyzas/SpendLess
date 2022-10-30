using System;
using System.Collections.Generic;

namespace SpendLess.Server.Models
{
    public partial class Finance
    {
        public int Id { get; set; }
        public string Category { get; set; } = null!;
        public string Tag { get; set; } = null!;
        public string? Comment { get; set; }
        public double Amount { get; set; }
        public int? HourInterval { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime? FixedDate { get; set; }
    }
}
