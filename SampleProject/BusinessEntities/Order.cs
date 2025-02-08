using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace BusinessEntities
{
    public class Order : IdObject
    {   
        public OrderStatus Status { get; set; } = OrderStatus.Pending;        
        public Guid CustomerId { get; set; }
        public User Customer { get; set; }
        
        //Keeping it simple for now and adding Shipping Address here only
        //Also not adding different fields for city, state, zip code etc.
        public string ShippingAddress { get; set; }
        public DateTime? ShippingDate { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DateTime CreatedDate { get; set; }
        //Calculated field
        public Decimal TotalPrice => CalculateTotalPrice();

        private Decimal CalculateTotalPrice()
        {
            var totalPrice = 0m;

            foreach (var item in Items) {
                totalPrice += item.TotalPrice();
            }

            return totalPrice;
        }
    }
}
