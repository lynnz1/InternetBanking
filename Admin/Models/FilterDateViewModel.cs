using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Admin.Models
{
	public class FilterDateViewModel
	{
		public int AccountNumber { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		[Display(Name ="Filter From:")]
		public DateTime? Start { get; set; }
		[DataType(DataType.Date)]
		[Display(Name = "To:")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? End { get; set; }

		public List<Transaction> Transactions { get; set; }
	}
}

