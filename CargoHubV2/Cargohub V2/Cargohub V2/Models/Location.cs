namespace Cargohub_V2.Models
{
    public class Location
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
    }
}
