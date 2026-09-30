using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Baganova.ViewModels
{
    public class CartVM
    {
        public int cartid { get; set; }
        public string img { get; set; }
        public string productname { get; set; }
        public int productid { get; set; }
        public int Instock { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public double total { get; set; }
    }
}