using System;
using s3512958_a2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using s3512958_a2.Data;
namespace s3512958_a2.Controllers
{
	public static class PopulateSelectLists
	{
       
        public static List<SelectListItem> AccountSelectList(Customer customer)
        {
            List<SelectListItem> accountItems = new List<SelectListItem>();
            foreach (var account in customer.Accounts)
            {
                accountItems.Add(new SelectListItem
                {
                    Text = account.AccountNumber.ToString() + "(" + account.AccountType + ")",
                    Value = account.AccountNumber.ToString()

                });
            }
            return accountItems;
        }

        public static List<SelectListItem> PayeeSelectList(List<Payee> allPayee)
        {
            List<SelectListItem> payeeItems = new List<SelectListItem>();
            
            foreach (var payee in allPayee)
            {
                payeeItems.Add(
                    new SelectListItem
                    {
                        Text = payee.Name,
                        Value = payee.PayeeID.ToString()
                    });
            }
            return payeeItems;
        }

        public static List<SelectListItem> PeriodSelectList()
        {
            List<SelectListItem> periodItems = new List<SelectListItem>();
            periodItems.Add(
                new SelectListItem
                {
                    Text = "One-Off Payment",
                    Value = "O"
                });
            periodItems.Add(
                new SelectListItem
                {
                    Text = "Monthly Payment",
                    Value = "M"
                });

            return periodItems;
        }
         
        
    }
}

