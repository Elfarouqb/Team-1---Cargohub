using Microsoft.AspNetCore.Http;

namespace Cargohub_V2.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int SourceId { get; set; }
        public string? OrderDate { get; set; }
        public string? RequestDate { get; set; }
        public string? Reference { get; set; }
        public string? Reference_extra { get; set; }
        public string? Order_status { get; set; }
        public string? Notes { get; set; }
        public string? ShippingNotes { get; set; }
        public string? PickingNotes { get; set; }
        public int WarehouseId { get; set; }
        public string? ShipTo { get; set; }
        public string? BillTo { get; set; }
        public int ShipmentId { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalTax { get; set; }
        public double TotalSurcharge { get; set; }
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public List<OrderStock> Stocks { get; set; } = new List<OrderStock>();


    }
}
