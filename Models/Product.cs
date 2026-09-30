using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Baganova.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Product ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100, ErrorMessage = "Product Name cannot exceed 100 characters.")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Photo")]
        public string Photo1 { get; set; }

        [Display(Name = "Photo 2")]
        public string Photo2 { get; set; }

        [Display(Name = "Photo 3")]
        public string Photo3 { get; set; }

        [Display(Name = "Photo 4")]
        public string Photo4 { get; set; }

        [Display(Name = "Photo 5")]
        public string Photo5 { get; set; }

        [Display(Name = "Photo 6")]
        public string Photo6 { get; set; }

        [Display(Name = "Photo 7")]
        public string Photo7 { get; set; }

        [Display(Name = "Photo 8")]
        public string Photo8 { get; set; }

        [Display(Name = "Photo 9")]
        public string Photo9 { get; set; }

        [Display(Name = "Photo 10")]
        public string Photo10 { get; set; }

        [StringLength(1000, ErrorMessage = "Product Details cannot exceed 1000 characters.")]
        [Display(Name = "Product Details")]
        public string ProductDetails { get; set; }

        [Display(Name = "Beneficial For")]
        public string BeneficialFor { get; set; }

        [Display(Name = "Product Information")]
        public string ProductInformation { get; set; }

        [Display(Name = "Dimensions")]
        public string Usage { get; set; }

        [Display(Name = "Net Weight")]
        public string Quantity { get; set; }

        //[Display(Name = "Specification")]
        [Display(Name = "Origin")]
        public string Specification { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        [Display(Name = "Price")]
        public double Price { get; set; }
        
        [Display(Name = "Wholesale Price")]
        public double? WholesalePrice { get; set; }
        
        [Display(Name = "Minimum Order Quantity")]
        public int? MinimumOrderQuantity { get; set; }

        [StringLength(50, ErrorMessage = "SKU Number cannot exceed 50 characters.")]
        [Display(Name = "SKU Number")]
        public string SKUNo { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "In Stock must be a non-negative number.")]
        [Display(Name = "In Stock")]
        public int InStock { get; set; }

        [StringLength(1000, ErrorMessage = "Product Description cannot exceed 1000 characters.")]
        [Display(Name = "Materials/Ingredients")]
        public string ProductDescription { get; set; }
        
        [Display(Name = "Scent/Fragrance")]
        public string Fragrance { get; set; }
        
        [Display(Name = "Is Featured")]
        public bool IsFeatured { get; set; }
        
        [Display(Name = "Is New Arrival")]
        public bool IsNewArrival { get; set; }
        
        [Display(Name = "Is Bestseller")]
        public bool IsBestseller { get; set; }
        
        [Display(Name = "How to Use")]
        [DataType(DataType.MultilineText)]
        public string HowToUse { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Updated Date")]
        public DateTime UpdatedDate { get; set; }
        
        // Virtual navigation properties
        public virtual ICollection<ProductAttributeValue> AttributeValues { get; set; }
    }
}