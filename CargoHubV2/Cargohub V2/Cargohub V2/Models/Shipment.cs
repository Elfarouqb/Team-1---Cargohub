namespace Cargohub_V2.Models
{
    public class Shipment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int SourceId { get; set; }
        public string? OrderDate { get; set; }
        public string? RequestDate { get; set; }
        public string? ShipmentDate { get; set; }
        public string? ShipmentType { get; set; }
        public string? ShipmentStatus { get; set; }
        public string? Notes { get; set; }
        public string? CarrierCode { get; set; }
        public string? CarrierDescription { get; set; }
        public string? ServiceCode { get; set; }
        public string? PaymentType { get; set; }
        public string? TransferMode { get; set; }
        public int TotalPackageCount { get; set; }
        public double TotalPackageWeight { get; set; }
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public List<ShipmentStock> Stocks { get; set; } = new List<ShipmentStock>();
    }
}
