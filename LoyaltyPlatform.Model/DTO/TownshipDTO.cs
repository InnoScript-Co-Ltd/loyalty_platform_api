using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.Model.DTO
{
    public class TownshipDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CityId { get; set; }
        public string? CityName { get; set; }
        [Required]
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        [Required]
        public int StateId { get; set; }
        public string? StateName { get; set; }
    }
}
