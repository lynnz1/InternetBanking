using System;
namespace s3512958_a2.Models
{
	public class TransactionViewModel
	{
		public int AccountNumber { get; set; }
		public char AccountType { get; set; }
		// Deposit/Withdraw/Transfer
		public string ActionType { get; set; }
		public decimal Amount { get; set; }
		public string Comment { get; set; }
		public decimal ServiceFee { get; set; }

		public string AccountNameString()
		{
			if (AccountType == 'S')
			{
				return "Saving Account";
			}
			else
			{
				return "Checking Account";
			}
		}
	}

	

}

