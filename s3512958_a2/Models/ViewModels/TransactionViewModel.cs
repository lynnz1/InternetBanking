using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace s3512958_a2.Models
{
	public class TransactionViewModel
	{
		public int AccountNumber { get; set; }
		public char AccountType { get; set; }
		// Deposit/Withdraw/Transfer
		public string ActionType { get; set; }
		[DataType(DataType.Currency)]
		public decimal Amount { get; set; }
		public string Comment { get; set; }
		[DataType(DataType.Currency)]
		public decimal ServiceFee { get; set; }
		public int? DesAccount { get; set; }

		public string AccountTypeString()
		{
			if (AccountType.Equals('S'))
			{
				return "Saving Account";
			}
			else
			{
				return "Checking Account";
			}
		}

		public decimal GetServiceFee (int numOfTransaction)
        {
			decimal fee = 0;
            if (numOfTransaction >= 2)
            {
				if (ActionType.Equals("Withdraw"))
				{
					fee = 0.05m;
					return fee;
				}
                if (ActionType.Equals("Transfer"))
                {
					fee = 0.10m;
					return fee;
                }

				return fee;
			}
			return fee;
            
        }

	}

	

}

