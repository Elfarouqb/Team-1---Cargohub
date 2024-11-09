namespace Cargohub_V2.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string? UId { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Shortescription { get; set; }
        public string? UpcCode { get; set; }
        public string? ModelNumber { get; set; }
        public string? CommodityCode { get; set; }
        public Item_Line? ItemLine { get; set; }
        public int? ItemLineId { get; set; }
       public Item_Group? ItemGroup { get; set; }
        public int? ItemGroupId { get; set; }
        public Item_Type? ItemType { get; set; }
        public int? ItemTypeId { get; set; }
        public int UnitPurchaseQuantity { get; set; }
        public int UnitOrderQuantity { get; set; }
        public int PackOrderQuantity { get; set; }
        public Supplier? Supplier { get; set; }
        public int Supplier_id { get; set; }
        public string? SupplierCode { get; set; }
        public string? SupplierPartNumber { get; set; }
        public string? Created_at { get; set; }
        public string? Updated_at { get; set; }
    }
}
