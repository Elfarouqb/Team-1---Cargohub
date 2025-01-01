using Cargohub_V2.DataConverters;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cargohub_V2.Models
{
    public class Shipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generated
        public int Id { get; set; }

        // Apply the SingleOrArrayConverter to handle both single value and array cases
        [JsonPropertyName("order_id")]
        [JsonConverter(typeof(SingleOrArrayConverter))] // This is the key change
        [Column(TypeName = "jsonb")] // JSONB column in DB
        public List<int> OrderId { get; set; } = new List<int>();

        [JsonPropertyName("source_id")]
        public int SourceId { get; set; }

        [JsonPropertyName("order_date")]
        public string? OrderDate { get; set; }

        [JsonPropertyName("request_date")]
        public string? RequestDate { get; set; }

        [JsonPropertyName("shipment_date")]
        public string? ShipmentDate { get; set; }

        [JsonPropertyName("shipment_type")]
        public string? ShipmentType { get; set; }

        [JsonPropertyName("shipment_status")]
        public string? ShipmentStatus { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }

        [JsonPropertyName("carrier_code")]
        public string? CarrierCode { get; set; }

        [JsonPropertyName("carrier_description")]
        public string? CarrierDescription { get; set; }

        [JsonPropertyName("service_code")]
        public string? ServiceCode { get; set; }

        [JsonPropertyName("payment_type")]
        public string? PaymentType { get; set; }

        [JsonPropertyName("transfer_mode")]
        public string? TransferMode { get; set; }

        [JsonPropertyName("total_package_count")]
        public int TotalPackageCount { get; set; }

        [JsonPropertyName("total_package_weight")]
        public double TotalPackageWeight { get; set; }

        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(FlexibleDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        [JsonConverter(typeof(FlexibleDateTimeConverter))]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("items")]
        public ICollection<ShipmentItem> Items { get; set; }
    }

    public class ShipmentItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonPropertyName("item_id")]
        public string ItemId { get; set; }

        public int Amount { get; set; }

        // Foreign Key
        [JsonPropertyName("shipment_id")]
        public int ShipmentId { get; set; } // This should link to the parent shipment
    }
}

