using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Baganova.Models
{
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Brand ID")]

        public int ID { get; set; }

        [Display(Name = "Brand Name")]

        public string BrandName { get; set; }

        [Display(Name = "Description")]

        public string Description { get; set; }

        [Display(Name = "Home Photo")]
        public string HomePhoto { get; set; }

        [Display(Name = "Brand Photo")]
        public string BrandPhoto { get; set; }
    }
}