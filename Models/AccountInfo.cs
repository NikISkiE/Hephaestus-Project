using Microsoft.Build.Framework;
using System.ComponentModel;

namespace Hephaestus_Project.Models
{
    public class AccountInfo
    {
        public string Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [DisplayName("Permission")]
        public string PermLVL { get; set; }
        public string Created_At { get; set; }
        public string UserID { get; set; } = "NULL";
    }
}
