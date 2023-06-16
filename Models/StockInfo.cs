using Microsoft.Build.Framework;

namespace Hephaestus_Project.Models
{
    public class StockInfo
    {
        [Required]
        public string Serial { get; set; }
        public string UserName { get; set; }
        [Required]
        public string InMaintance { get; set; }
        public string ID { get; set; }
    }
}
