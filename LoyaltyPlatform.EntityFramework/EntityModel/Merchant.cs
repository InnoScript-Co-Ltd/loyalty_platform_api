using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.EntityFramework.EntityModel
{
    public class Merchant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Profile { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PrefixPhoneNumber { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime EmailVerifiedDate { get; set; }
        public DateTime PhoneVerifiedDate { get; set; }
    }
}
