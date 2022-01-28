using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace s3512958_a2.Models
{
	public class BillPayViewModel
	{
        public List<BillPay> Billpays { get; set; }

		public string SelectedPayee { get; set; }
		public List<SelectListItem> PayeeSelectList { get; set; }
		public Payee Payee { get; set; }

		public int SelectedAccount { get; set; }
		public List<SelectListItem> AccountSelectList { get; set; }
		public Account Account { get; set; }
    }
}

