﻿using Hephaestus_Project.Interface;
using Microsoft.Build.Framework;

namespace Hephaestus_Project.Models
{
    public class DivisionInfo : IDatabase.IDivisionInfo
    {
        public string Id { get; set; }
        [Required]
        public string  Name{ get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Division { get; set; }
        public string Rank { get; set; }
    }
}
