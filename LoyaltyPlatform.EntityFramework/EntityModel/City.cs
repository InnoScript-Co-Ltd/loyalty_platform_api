using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.EntityFramework.EntityModel
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }
        public virtual State State { get; set; }
    }
}
