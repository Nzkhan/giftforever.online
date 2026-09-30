using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Baganova.Models
{
    public class PurestContext : DbContext
    {
        public PurestContext() : base("DefaultConnection")
        {

        }

        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Video> Videos { get; set; }
        //public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ProductProductTypeMapping> ProductProductTypeMappings { get; set; }
        public DbSet<Blog> blogs { get; set; }
        public DbSet<Tag> tags { get; set; }
        public DbSet<MappingBlogTag> mappingBlogTags { get; set; }
        public DbSet<TransactionDetails> TransactionDetails { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }

    }
}