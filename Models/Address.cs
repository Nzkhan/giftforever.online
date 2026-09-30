using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Baganova.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Address ID")]
        public int ID { get; set; }

        [Display(Name = "Address")]
        public string address { get; set; }

        public virtual int UserDetailsID { get; set; }

        [ForeignKey("UserDetailsID")]
        public UserDetail userdetails { get; set; }

        public bool IsActive { get; set; }
    }
}