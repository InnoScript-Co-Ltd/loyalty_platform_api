using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.Model.DTO
{
    public class MerchantDTO
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Profile { get; set; }
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PrefixPhoneNumber { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public DateTime EmailVerifiedDate { get; set; }
        public DateTime PhoneVerifiedDate { get; set; }
    }
}
