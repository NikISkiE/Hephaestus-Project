using Microsoft.Build.Framework;

namespace Hephaestus_Project.Models
{
    public class MyEQinfo
    {
        public string Id { get; set; }
        [Required]
        public string Serial { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public bool InMaintance { get; set; }

    }
}
