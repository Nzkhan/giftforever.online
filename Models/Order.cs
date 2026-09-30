
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Baganova.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Display(Name = "Order ID")]
        public int ID { get; set; }

        [Display(Name = "Order Date")]
        public DateTime CreatedOn { get; set; }

        public string Status { get; set; }

        [Display(Name = "Delivery Charges")]

        public double DeliveryCharges { get; set; }

        [Display(Name = "Total Amount")]

        public double TotalAmount { get; set; }

        public virtual int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User user { get; set; }

        public int AddressID { get; set; }
        //  public string AddressID { get; set; }
        public string PaymentMode { get; set; }

        public int TransactionId { get; set; }

        //[ForeignKey("TransactionId")]
      //  public virtual TransactionDetails transactionDetails { get; set; }

    //[ForeignKey("AddressID")]
    //public virtual Address address { get; set; }

}
}