using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InternetBanking.Models
{
	public class StatementViewModel
	{
		public int AccountNumber { get; set; }

		[DataType(DataType.Currency)]
		public decimal Balance { get; set; }
		public List<Transaction> Transactions { get; set; }
		public int CurrentPage { get; set; }
		public bool LastPage { get; set; }

	}
}

