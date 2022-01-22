using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace s3512958_a2.Models
{
    public class Login
    {
        [Column(TypeName = "char")]
        [StringLength(8)]
        public string LoginID { get; set; }
        //8 Digits only

        [Required]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        [Column(TypeName = "char")]
        [StringLength(64)]
        public string PasswordHash { get; set; }

    }
}

