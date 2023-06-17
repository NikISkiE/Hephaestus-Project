using Hephaestus_Project.Interface;
using Microsoft.Build.Framework;

namespace Hephaestus_Project.Models
{
    public class UserInfo : IDatabase.IUserInfo
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
        public int IsRegistered { get; set; } = 0;

    }
}
