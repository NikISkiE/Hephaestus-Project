using Microsoft.Build.Framework;

namespace Hephaestus_Project.Models
{
    public class MyEQinfo
    {
        public string Serial { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }

    }
}
