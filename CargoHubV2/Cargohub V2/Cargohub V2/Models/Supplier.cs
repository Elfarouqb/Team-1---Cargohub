namespace Cargohub_V2.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? AddressExtra { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? Province { get; set; }
        public string? Country { get; set; }
        public string? ContactName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Reference { get; set; }
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
    }
}
