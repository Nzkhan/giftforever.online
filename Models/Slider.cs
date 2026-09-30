using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Baganova.Models
{
    public class Slider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Slider Photo")]
        public string Photo { get; set; }

        [Display(Name = "Slider Title")]
        public string Title { get; set; }

        [Display(Name = "Slider SubTitle")]
        public string SubTitle { get; set; }
    }
}