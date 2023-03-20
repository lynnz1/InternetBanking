using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetBanking.Models
{
	public class BillPayViewModel
	{
		public int SelectedBillPayID { get; set; }
        public virtual List<BillPay> Billpays { get; set; }

		public int SelectedPayee { get; set; }
		public virtual List<SelectListItem> PayeeSelectList { get; set; }
		public Payee Payee { get; set; }

		public int SelectedAccount { get; set; }
		public virtual List<SelectListItem> AccountSelectList { get; set; }
		public Account Account { get; set; }

		public char SelectedPeriod { get; set; }
		public virtual List<SelectListItem> Period { get; set; }


		public string PeriodString(char period)
        {
            if (period == 'M')
            {
				return "Monthly";
            }
            else
            {
				return "One-Off";
            }
        }
	}

	
}

