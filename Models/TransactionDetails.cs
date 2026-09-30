using Baganova.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Baganova.Models
{
    public class TransactionDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        public  int UserId { get; set; }

     
       // public virtual User user { get; set; }

        public virtual int OrderID { get; set; }

        //[ForeignKey("UserId")]
        [ForeignKey("OrderID")]
        public virtual Order order { get; set; }



        public string Status { get; set; }

        public string ReceiptURL { get; set; }

        public string PaymentModel { get; set; }

        public string PaymentIntentId { get; set; }

        public string PaymentMethod { get; set; }

        public string ChargeId { get; set; }

        public bool IsPaid { get; set; }

        public string PaymentType { get; set; }
        public string BalanceTransactionId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string UserphoneNumber { get; set; }
        public string UserCountry { get; set; }
        public string UserCity { get; set; }
        public string UserState { get; set; }
        public string Currency { get; set; }
        public string PaymentFailedTransactionId { get; set; }
        public string PaymentFailedCode { get; set; }
        public string PaymentFailedMessage { get; set; }
    }
}