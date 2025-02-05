using BusinessEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models.Products
{
    public class ProductData :IdObjectData
    {
        public ProductData(Product product) : base(product) {
            ProductName = product.Name;
            ProductDescription = product.Description;
            Price = product.Price;
            StockQuantity = product.StockQuantity;
        }        
            
        public string ProductName { get; set; }        
        public string ProductDescription { get; set; }        
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    
    }
}