using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.Model.DTO
{
    public class StateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ZipCode { get; set; }
        public string Profile { get; set; }
        public int CountryId { get; set; }
    }
}
