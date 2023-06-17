using Hephaestus_Project.Interface;
using Microsoft.Build.Framework;

namespace Hephaestus_Project.Models
{
    public class StockInfo : UserInfo, IDatabase.StockInfo
    {
        [Required]
        public string Serial { get; set; }
        public string UserIDL { get; set; }
        [Required]
        public string EquipmentID { get; set; }
        public string InMaintance { get; set; }
        public string ID { get; set; }
    }
}
