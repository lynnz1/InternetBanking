using System;
namespace s3512958_a2.Models
{
	public class TransactionViewModel
	{
		public int AccountNumber { get; set; }
		public char AccountType { get; set; }
		public string ActionType { get; set; }
		public decimal Amount { get; set; }
		public string Comment { get; set; }
	}
}

