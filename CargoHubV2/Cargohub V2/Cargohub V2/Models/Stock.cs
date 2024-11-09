﻿namespace Cargohub_V2.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string? ItemId { get; set; } 
        public int Quantity { get; set; }

    }

    public class OrderStock : Stock
    {
        public Order? Order { get; set; }
        public int OrderId { get; set; }
                                         
    }

    public class ShipmentStock : Stock
    {
        public Shipment? Shipment { get; set; }
        public int ShipmentId { get; set; }
                                            
    }

    public class TransferStock : Stock
    {
        public Transfer? Transfer { get; set; }
        public int TransferId { get; set; }
                                            
    }
}
