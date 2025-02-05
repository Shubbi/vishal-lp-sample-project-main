using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessEntities
{
    public class Product : IdObject
    {
        private string _name;
        private string _description;
        private decimal _price;
        private int _stockQuantity;


        public string Name {
            get
            {
                return _name;
            }
                
            set 
            { 
                if (string.IsNullOrWhiteSpace(value)) 
                    throw new ArgumentException("Name was not provided");
                _name = value;
            } 
        }

        public string Description
        {
            get 
            {  
                return _description; 
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) 
                    throw new ArgumentException("Description was not provided");

                _description = value;
            }
        }

        public decimal Price
        {
            get
            { 
                return _price; 
            }
            set
            {
                if (value <= 0m)
                    throw new ArgumentException("Price should be greater than 0");
                _price = value;
            }
        }

        public int StockQuantity
        {
            get
            {
                return _stockQuantity;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Stock Quantity cannot be negative");
                _stockQuantity = value;
            }
        }
    }

}
