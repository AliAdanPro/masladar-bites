
using System;

namespace BlazorApp23.Components.Models
{
    public class WeeklyProfit
    {
        public int Year { get; set; }
        public int WeekNumber { get; set; }
        public DateTime WeekStartDate { get; set; }
        public decimal TotalSales { get; set; }
        public int OrderCount { get; set; }
        public decimal Profit { get; set; }
        public string Status { get; set; }

        public string WeekRange =>
            $"{WeekStartDate:MMM dd} - {WeekStartDate.AddDays(6):MMM dd, yyyy}";

        public string WeekLabel =>
            $"Week {WeekNumber} ({WeekStartDate:yyyy})";
    }
}