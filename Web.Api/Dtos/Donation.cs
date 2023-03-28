using System;

namespace AmoToSheetFunc.Dtos
{
    public class Donation
    {
        public long AmoLeadId { get; set; }

        public string ContactName { get; set; } = string.Empty;

        public string? Amount { get; set; } = string.Empty;
        
        public DateTime Date { get; set; }
    }
}