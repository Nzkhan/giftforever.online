using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Baganova.Models
{
    public class ProductType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Product ID")]
        public int ID { get; set; }

        [Display(Name = "Type Name")]
        public string TypeName { get; set; }
        
        public string Description { get; set; }
        
        public string Photo { get; set; }
        
        public string HomePhoto { get; set; }
        
        [Display(Name = "Parent Category")]
        public int? ParentTypeID { get; set; }
        
        [ForeignKey("ParentTypeID")]
        public virtual ProductType ParentType { get; set; }
        
        public virtual ICollection<ProductType> SubTypes { get; set; }
        
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
        
        [Display(Name = "Show in Navigation")]
        public bool ShowInNavigation { get; set; } = true;
    }
}