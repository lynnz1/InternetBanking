using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System;
namespace Admin.Models
{
	public class Customer
	{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }
        [StringLength(11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Please enter a valid TFN of 11 digits")]
        public string? TFN { get; set; }
        [StringLength(50)]
        public string? Address { get; set; }
        [StringLength(40)]
        public string? Suburb { get; set; }
        [StringLength(3)]
        [RegularExpression(@"^(NSW)|(VIC)|(QLD)|(SA)|(NT)|(TAS)|(WA)|(ACT)$",
            ErrorMessage ="Please enter a valid Australian State. E.g.(VIC,NSW,QLD,SA,NT,TAS,WA,ACT)")]
        public string? State { get; set; }
        [StringLength(4)]
        [RegularExpression(@"^\d{4}$", ErrorMessage ="Please enter a valid postcode")]
        public string? PostCode { get; set; }
        [StringLength(12)]
        [RegularExpression(@"^(04)\d{2}( )\d{3}( )\d{3}$", ErrorMessage = "Please enter a valid mobile number. Format(04XX XXX XXX)")]
        public string? Mobile { get; set; }


        public virtual Login Login { get; set; }
        public virtual List<Account> Accounts { get; set; }
    }
}

