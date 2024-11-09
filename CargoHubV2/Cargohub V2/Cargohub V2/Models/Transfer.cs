using Microsoft.AspNetCore.Http.Features;

namespace Cargohub_V2.Models
{
    public class Transfer
    {
        public int Id { get; set; }
        public string? Reference { get; set; }
        public int TransferFrom { get; set; }
        public int TransferTo { get; set; }
        public string? TransferStatus { get; set; }
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public List<TransferStock>? Stocks { get; set; }
    }
}
