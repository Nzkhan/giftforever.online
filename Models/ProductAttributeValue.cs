using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Baganova.Models
{
    public class ProductAttributeValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        [Required]
        public int ProductID { get; set; }
        
        [Required]
        public int AttributeID { get; set; }
        
        [Required]
        public string Value { get; set; }
        
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
        
        [ForeignKey("AttributeID")]
        public virtual ProductAttribute Attribute { get; set; }
    }
}