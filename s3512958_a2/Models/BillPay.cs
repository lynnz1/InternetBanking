using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace s3512958_a2.Models
{
    public class BillPay
    {
        public int BillPayID { get; set; }

        [ForeignKey("Account")]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }

        [ForeignKey("Payee")]
        [Display(Name = "Payee")]
        public int PayeeID { get; set; }
        public virtual Payee Payee { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Scheduled Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime ScheduleTimeUtc { get; set; }
        public char Period { get; set; }

    }
}

