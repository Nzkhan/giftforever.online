using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Baganova.Models
{
    public class Testimonial
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       [Display(Name ="Client ID")]
       public int ClientId { get; set; }


        [Display(Name = "Client Name")]
        public string ClientName { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name ="Created Date")]
        public DateTime CreatedDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Updated Date")]
        public DateTime UpdatedDate { get; set; }


        [Display(Name = "Client Description")]
        public string ClientDescrpt { get; set; }
    }
}