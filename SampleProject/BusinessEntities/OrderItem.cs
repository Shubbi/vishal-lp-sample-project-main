using System;

namespace BusinessEntities
{
    public class OrderItem : IdObject
    {       
        public Guid ProductId { get; set; }

        //I'm bringing the price and productname here , as in the future the product price could change
        //so we still have the original price
        public string ProductName { get; set; }
        public Decimal ProductPrice { get; set; }

        public int Quantity { get; set; }

        public Decimal TotalPrice()
        {
            return Quantity * ProductPrice;
        }
        
    }
}
