﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.Model.DTO
{
    public class CountryDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string FlagIcon { get; set; }
        public string MobilePrefixNumber { get; set; }
    }
}
