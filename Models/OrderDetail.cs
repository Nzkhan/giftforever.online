using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Baganova.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Product")]
        public virtual int ProductsID { get; set; }

        public int Quantity { get; set; }

        public double Amount { get; set; }

        [ForeignKey("ProductsID")]
        public virtual Product product { get; set; }

        public virtual int OrderID { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order order { get; set; }

    }
}