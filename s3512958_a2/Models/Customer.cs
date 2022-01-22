﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace s3512958_a2.Models
{
	public class Customer
	{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }
        [StringLength(11)]
        public string? TFN { get; set; }
        [StringLength(50)]
        public string? Address { get; set; }
        [StringLength(40)]
        public string? City { get; set; }
        [StringLength(3)]
        public string? State { get; set; }
        [StringLength(4)]
        public string? Postcode { get; set; }
        [StringLength(12)]  
        public string? Mobile { get; set; }


        public Login Login { get; set; }
        public List<Account> Accounts { get; set; }
    }
}

