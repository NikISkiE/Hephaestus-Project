﻿using Hephaestus_Project.Interface;
using Microsoft.Build.Framework;

namespace Hephaestus_Project.Models
{
    public class ArsenalInfo : IDatabase.ArsenalInfo
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        public string Stocked { get; set; }
    }
}
