﻿namespace Cargohub_V2.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public string? ItemId { get; set; }
        public string? Description { get; set; }
        public string? ItemReference { get; set; }
        public List<int>? Locations { get; set; }
        public int TotalOnHand { get; set; }
        public int totalExpected { get; set; }
        public int TotalOrdered { get; set; }
        public int TotalAllocated { get; set; }
        public int TotalAvailable { get; set; }
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
    }
}
