using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.Model.DTO
{
    public class StateDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ZipCode { get; set; }
        public string Profile { get; set; }
        public int CountryId { get; set; }
    }
}
