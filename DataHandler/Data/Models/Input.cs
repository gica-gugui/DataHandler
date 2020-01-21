using DataHandler.Attributes;
using DataHandler.Data.Enums;
using System;

namespace DataHandler.Data.Models
{
    public class Input
    {
        public int Project { get; set; }

        public string Description { get; set; }

        [ImportName("Start date")]
        public DateTime StartDate { get; set; }

        public string Category { get; set; }

        public string Responsible { get; set; }

        [ImportName("Savings amount")]
        public decimal? SavingsAmount { get; set; }

        public string Currency { get; set; }

        public Complexity Complexity { get; set; }
    }
}
