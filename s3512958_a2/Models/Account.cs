﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InternetBanking.Models
{
	public partial class Account
	{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Display(Name = "Type")]
        [RegularExpression(@"^(C)|(S)|(c)|(s))$",
            ErrorMessage = "Please enter a valid Account Type, C or S")]
        public char AccountType { get; set; }

        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        //Count for the number of withdraw/transfer transaction of this account.
        public int NumOfTransactions { get; set; }

        public virtual List<Transaction> Transactions { get; set; }


        
    }


}

