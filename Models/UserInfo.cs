using Microsoft.Build.Framework;

namespace Hephaestus_Project.Models
{
    public class UserInfo
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Division { get; set; }
        [Required]
        public string Rank { get; set; }
        public string AccountID { get; set; } = "NULL";

    }
}
