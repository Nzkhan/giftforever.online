using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Baganova.Models
{
    public class ProductAttribute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        [Required]
        [Display(Name = "Attribute Name")]
        public string AttributeName { get; set; }
        
        [Display(Name = "Attribute Type")]
        public string AttributeType { get; set; } // e.g., "scent", "benefit", "material"
        
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
        
        [Display(Name = "Is Filterable")]
        public bool IsFilterable { get; set; } = true;
    }
}