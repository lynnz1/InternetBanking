using System;
namespace s3512958_a2.Models
{
	public partial class Account
	{
        public decimal CalculateBalance()
        {
            decimal balance = 0;
            foreach (var t in Transactions)
            {
                //To be used for initial JSON since transaction type was not provided.
                if (t.TransactionType == '\0')
                {
                    balance += t.Amount;
                }

                //Type of receiving transactions: D,T(without destination account)
                //Deposit
                else if (t.TransactionType.Equals('D'))
                {
                    balance += t.Amount;
                }
                //Transaction to this account
                else if (t.TransactionType.Equals('T') && t.DestinationAccountNumber == 0)
                {
                    balance += t.Amount;
                }
                //Withdraw, out-transaction and service fee.
                else
                {
                    balance -= t.Amount;
                }
            }
            return balance;
        }

        public string AccountNameString()
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

        public decimal AvailableBalance()
        {
            if (AccountType == 'S')
            {
                return Balance;
            }
            else
            {
                return Balance-=300;
            }
        }
    }
}

