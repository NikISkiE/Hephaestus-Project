using Microsoft.Build.Framework;

namespace Hephaestus_Project.Models
{
    public class DivisionInfo
    {
        public string Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PermLVL { get; set; }
        public string Created_At { get; set; }
        public string UserID { get; set; } = "NULL";
    }
}
