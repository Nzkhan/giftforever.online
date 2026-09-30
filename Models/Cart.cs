using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Baganova.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Display(Name = "Chart ID")]

        public int ID { get; set; }

        [Display(Name = "User Name")]

        public virtual int UserID { get; set; }

        [Display(Name = "Product Name")]

        public virtual int ProductID { get; set; }

        public int Quantity { get; set; }

      //  public virtual int RoleID { get; set; }

        [ForeignKey("UserID")]
        public virtual User user { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product product { get; set; }

    }
}