using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace s3512958_a2.Models
{
	public class AmountCommentViewModel
	{
		[DataType(DataType.Currency), Range(0,double.MaxValue)]
		public decimal Amount { get; set; }
		[StringLength(30)]
		public string Comment { get; set; }
		public string ActionType { get; set; }
		public int? DesAccount { get; set; }
	}
}

