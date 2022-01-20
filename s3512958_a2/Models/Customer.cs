using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace s3512958_a2.Models
{
	public class Customer
	{
        public int CustomerID { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }
        [StringLength(11)]
        public string? TFN { get; set; }
        [StringLength(50)]
        public string? Address { get; set; }
        [StringLength(40)]
        public string? Suburb { get; set; }
        [StringLength(3)]
        [RegularExpression(@"^(NSW)|(VIC)|(QLD)|(SA)|(NT)|(TAS)|(WA)|(ACT)$")]
        public string? State { get; set; }
        [StringLength(4)]
        [RegularExpression(@"^\d{4}$")]
        public string? Postcode { get; set; }
        [StringLength(12)]
        [RegularExpression(@"^(04)\d{2}( )\d{3}( )\d{3}$")]
        public string? Mobile { get; set; }


        public Login Login { get; set; }
    }
}

